# FlockBuddy

FlockBuddy is a .NET library for adding flocking / steering-behavior AI to MonoGame projects. It implements the classic Craig Reynolds-style "boids" model (separation, alignment, cohesion) plus a larger set of steering behaviors (seek, flee, pursuit, evade, wall, and more) that you can mix and weight per flock.

It is one of several small "Buddy" libraries by
[dmanning23](https://github.com/dmanning23) that plug into MonoGame projects;
FlockBuddy handles the AI movement, and you own rendering, input, and the rest of your game.

## Features

- Ready-to-use steering behaviors: `WallAvoidance`, `ObstacleAvoidance`,
  `Evade`, `Flee`, `Direction`, `Separation`, `Alignment`, `Cohesion`,
  `Seek`, `Arrive`, `Wander`, `Pursuit`, `OffsetPursuit`, `Interpose`,
  `GuardSeparation`, `GuardAlignment`, `GuardCohesion`, `Hide`, 
  `FollowPath`. (See [docs/behaviors.md](docs/behaviors.md) for the status
  of each — a few are not yet implemented upstream.)
- Weighted, prioritized, or dithered force summing so you can tune how
  behaviors combine.
- Predator / prey / VIP relationships between flocks, for chase, flee, and
  guard scenarios.
- Wall and obstacle avoidance, waypoint path following, and optional
  toroidal ("wrap around the screen") world bounds.
- Optional cell-space partitioning (via
  [CellSpacePartition](https://www.nuget.org/packages/CellSpacePartition))
  for faster neighbor queries with large flocks.

## Requirements

- .NET 8.0 SDK or later
- [MonoGame](https://www.monogame.net/) 3.8.x (DesktopGL)

## Install

FlockBuddy is published on NuGet:

```
dotnet add package FlockBuddy
```

Or add it directly to your `.csproj`:

```xml
<PackageReference Include="FlockBuddy" Version="5.*" />
```

## Quick start

The core building blocks are a `Flock` (holds the boids and the world data: walls, obstacles, waypoints) and a `FlockManager` (holds the tuning parameters and behaviors, and creates boids for you).

```csharp
using FlockBuddy;
using Microsoft.Xna.Framework;

// 1. Create a flock and its manager
var flock = new Flock();
var flockManager = new FlockManager(flock);

// 2. Turn on the behaviors you want, and how strongly each should pull
flockManager.AddBehavior(BehaviorType.Separation, 60f);
flockManager.AddBehavior(BehaviorType.Alignment, 10f);
flockManager.AddBehavior(BehaviorType.Cohesion, 1f);
flockManager.AddBehavior(BehaviorType.Wander, 1f);

// 3. Add some boids
for (int i = 0; i < 30; i++)
{
    flockManager.AddBoid(new Vector2(100, 100), Vector2.UnitX);
}

// 4. Every frame: update the flock, then draw it however you like
GameTimer.GameClock gameClock = new GameTimer.GameClock();

protected override void Update(GameTime gameTime)
{
    gameClock.Update(gameTime);
    flock.Update(gameClock);
}

protected override void Draw(GameTime gameTime)
{
    flock.Draw(primitive, Color.White); // an IPrimitive from PrimitiveBuddy, or draw the boids yourself
}
```

For a walkthrough of every step — including walls, obstacles, predator/prey
setup, and tuning — see [docs/getting-started.md](docs/getting-started.md).

## Documentation

Full documentation lives in [docs/](docs/):

- [Getting Started](docs/getting-started.md) — install, setup, and a complete usage walkthrough
- [Core Concepts](docs/core-concepts.md) — how `Flock`, `FlockManager`, `Boid`, and behaviors fit together
- [Steering Behaviors](docs/behaviors.md) — what each behavior does, its default weight, and implementation status
- [Configuration Reference](docs/configuration-reference.md) — every tunable boid parameter
- [Advanced Topics](docs/advanced-topics.md) — walls, obstacles, waypoints, predator/prey/VIP groups, world wrap, cell-space partitioning

## Example project

`FlockBuddyExample/` contains a MonoGame demo app that drives a set of flocks through a UI built on `MenuBuddy`. As checked into this repository it does not currently build standalone (it references a `FlockBuddyWidgets` assembly that isn't wired up as a dependency here) — treat it as a reference for wiring behaviors into a real game screen rather than a `dotnet run`-ready sample.

## License

MIT — see [LICENSE](LICENSE).
