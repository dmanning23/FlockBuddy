# Getting Started

This walks through installing FlockBuddy, wiring it into a MonoGame
`Game`/`GameComponent`, and building a working flock from scratch.

## 1. Install

FlockBuddy targets **.NET 8.0** and depends on `MonoGame.Framework.DesktopGL`
3.8.x plus a handful of small companion libraries (`CellSpacePartition`,
`CollisionBuddy`, `GameTimer`, `MatrixExtensions`, `PrimitiveBuddy`,
`RandomExtensions.dmanning23`, `Vector2Extensions`). NuGet pulls all of these
in automatically.

```
dotnet add package FlockBuddy
```

or in your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="FlockBuddy" Version="5.*" />
</ItemGroup>
```

## 2. Create a `Flock` and a `FlockManager`

A **`Flock`** is the data container: the list of boids, walls, obstacles,
waypoints, and relationships to other flocks (predators/prey/VIPs).

A **`FlockManager`** is the configuration/control surface: it holds the
tuning parameters (speed, mass, turn rate, query radii, etc.) and the list
of active behaviors, and it's the factory you use to add boids so every new
boid picks up the current settings automatically.

```csharp
using FlockBuddy;

var flock = new Flock();
var flockManager = new FlockManager(flock);
```

You'll generally want one `Flock`/`FlockManager` pair per group of agents
that should behave the same way (e.g. one for a bird flock, a separate one
for a pack of enemies chasing the player).

## 3. Choose and weight your behaviors

Behaviors are added by `BehaviorType` with a weight that controls how
strongly they pull on the boid relative to the other active behaviors (see
[Steering Behaviors](behaviors.md) for what each one does and
[Configuration Reference](configuration-reference.md#default-behavior-weights)
for the built-in default weights).

```csharp
// classic "boids" flocking
flockManager.AddBehavior(BehaviorType.Separation);  // steer away from close neighbors
flockManager.AddBehavior(BehaviorType.Alignment);   // match neighbors' heading
flockManager.AddBehavior(BehaviorType.Cohesion);    // steer toward the group's center

// give them something to do when nothing else is happening
flockManager.AddBehavior(BehaviorType.Wander);
```

`AddBehavior(BehaviorType)` uses the library's built-in default weight for
that behavior. Pass an explicit weight as a second argument to override it:

```csharp
flockManager.AddBehavior(BehaviorType.Separation, 80f);
```

> **Note:** both `FlockManager` and each individual `Boid` keep their
> behavior lists keyed by `BehaviorType`, so calling `AddBehavior` again for
> a type you've already added doesn't create a duplicate — it just retunes
> the live weight on every boid. You can also call
> `SetBehaviorWeight(BehaviorType, float)` directly if you just want to
> retune an already-added behavior without going through `AddBehavior`.

Use `RemoveBehavior(BehaviorType)` to turn a behavior off, and
`HasBehavior` / `GetAllBehaviors` / `GetBehaviorWeight` to inspect the
current setup.

Behaviors you add to the `FlockManager` before adding boids are applied to
every boid it creates from then on; adding a behavior after boids already
exist retroactively adds it to all of them too.

## 4. Add boids

```csharp
using Microsoft.Xna.Framework;

for (int i = 0; i < 30; i++)
{
    var position = new Vector2(100 + i * 5, 100);
    var heading = Vector2.UnitX; // must be a unit vector
    flockManager.AddBoid(position, heading);
}
```

`FlockManager.AddBoid` creates the `Boid`, adds it to the `Flock`, applies
every currently-configured tuning parameter (see
[Configuration Reference](configuration-reference.md)), and wires up all of
the behaviors you registered on the manager.

## 5. Update and draw every frame

FlockBuddy uses `GameTimer.GameClock` (from the companion `GameTimer`
package) instead of raw MonoGame `GameTime`, so behaviors like `Wander` and
the retarget timer stay frame-rate independent.

```csharp
using GameTimer;

private readonly GameClock _flockClock = new GameClock();

protected override void Update(GameTime gameTime)
{
    _flockClock.Update(gameTime);
    flock.Update(_flockClock);

    base.Update(gameTime);
}
```

Drawing is optional and only used for debug visualization — FlockBuddy has
no rendering opinions beyond a couple of helper methods that draw through
[PrimitiveBuddy](https://www.nuget.org/packages/PrimitiveBuddy)'s
`IPrimitive` abstraction:

```csharp
protected override void Draw(GameTime gameTime)
{
    flock.Draw(primitive, Color.White);        // circle + heading line per boid
    flock.DrawWalls(primitive);                // wall segments, if any
    flock.DrawWhiskers(primitive, Color.Yellow); // WallAvoidance feelers, if that behavior is active
    flock.DrawCells(primitive);                 // cell-space partition grid, if enabled

    base.Draw(gameTime);
}
```

In a real game you'd normally draw your own sprites at each boid's
`Position`/`Rotation` instead of (or in addition to) these debug primitives
— see [Core Concepts](core-concepts.md#reading-boid-state-for-rendering).

## 6. Put up walls (optional)

If your boids should stay inside the screen (or a room), give the flock
some walls. FlockBuddy can generate a rectangle of walls for you:

```csharp
var screenBounds = new Rectangle(0, 0, 1920, 1080);
flockManager.AddDefaultWalls(DefaultWalls.All, screenBounds);
```

This only *creates* walls — for boids to react to them you also need the
`WallAvoidance` behavior turned on (see [Advanced Topics](advanced-topics.md#walls)):

```csharp
flockManager.AddBehavior(BehaviorType.WallAvoidance);
```

## 7. A complete minimal example

```csharp
using FlockBuddy;
using GameTimer;
using Microsoft.Xna.Framework;

public class FlockDemo
{
    private readonly Flock _flock = new Flock();
    private readonly FlockManager _manager;
    private readonly GameClock _clock = new GameClock();

    public FlockDemo(Rectangle bounds)
    {
        _manager = new FlockManager(_flock);

        _manager.AddDefaultWalls(DefaultWalls.All, bounds);
        _manager.AddBehavior(BehaviorType.WallAvoidance);
        _manager.AddBehavior(BehaviorType.Separation);
        _manager.AddBehavior(BehaviorType.Alignment);
        _manager.AddBehavior(BehaviorType.Cohesion);

        for (int i = 0; i < 40; i++)
        {
            _manager.AddBoid(
                new Vector2(bounds.Center.X, bounds.Center.Y),
                Vector2.UnitX);
        }
    }

    public void Update(GameTime gameTime)
    {
        _clock.Update(gameTime);
        _flock.Update(_clock);
    }
}
```

## Next steps

- [Core Concepts](core-concepts.md) explains how `Boid`, `Mover`,
  `BaseEntity`, and the behavior pipeline fit together, and how the three
  force-summing strategies differ.
- [Steering Behaviors](behaviors.md) documents every behavior in detail,
  including which ones are still stubs upstream.
- [Configuration Reference](configuration-reference.md) lists every tunable
  parameter and its default.
- [Advanced Topics](advanced-topics.md) covers walls, obstacles, waypoint
  paths, predator/prey/VIP flock relationships, world wrapping, and
  cell-space partitioning for large flocks.
