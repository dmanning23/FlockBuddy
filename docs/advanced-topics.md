# Advanced Topics

## Walls

Walls are line segments (`CollisionBuddy.ILine`) stored on `Flock.Walls`
that the `WallAvoidance` behavior steers away from. The easiest way to set
them up is to let FlockBuddy build a rectangle for you:

```csharp
var bounds = new Rectangle(0, 0, 1920, 1080);
flockManager.AddDefaultWalls(DefaultWalls.All, bounds);
flockManager.AddBehavior(BehaviorType.WallAvoidance);
```

`DefaultWalls` controls which edges get walls:

| Value | Effect |
|---|---|
| `None` (default) | No walls (`Flock.Walls` becomes an empty list). |
| `All` | Walls on all four sides of `bounds`. |
| `TopBottom` | Only the top and bottom edges. |
| `LeftRight` | Only the left and right edges. |

Internally this uses `CollisionBuddy.Line.ExtendedInsideRect(rect, 128f)`,
which extends each wall segment 128 units past the rectangle's corners so a
boid's feelers can't sneak past a corner gap. If you need custom wall
geometry (not just a rectangle), build your own `List<ILine>` and assign it
directly to `flock.Walls`/`flockManager.Walls` instead of using
`AddDefaultWalls`.

Note that adding walls to the flock does nothing on its own — a boid only
reacts to them if the `WallAvoidance` behavior is active. You can draw the
wall segments for debugging with `flock.DrawWalls(primitive)` and the
per-boid detection feelers with
`flock.DrawWhiskers(primitive, color)`.

## Obstacles

Obstacles are anything implementing `IBaseEntity` (just `Position` +
`Radius`) added to `Flock.Obstacles`:

```csharp
flock.Obstacles = new List<IBaseEntity>
{
    new BaseEntity(new Vector2(400, 300), radius: 50f),
    // ...or your own game object, as long as it implements IBaseEntity
};
flockManager.AddBehavior(BehaviorType.ObstacleAvoidance);
```

Unlike walls, obstacles aren't limited to level geometry — anything that
should be dodged (a turret, a hazard, another game entity) can be added
here. `Flock.Obstacles` is a plain list you manage yourself; there's no
`AddObstacle` helper, and there's no automatic removal, so remove entries
yourself when an obstacle goes away.

## Waypoint paths

`FollowPath` walks a boid through `Flock.Waypoints` in order:

```csharp
flock.Waypoints = new List<Vector2>
{
    new Vector2(100, 100),
    new Vector2(500, 100),
    new Vector2(500, 500),
};
flockManager.AddBehavior(BehaviorType.FollowPath);
```

Each boid tracks its own progress through the path independently. Once a
boid passes the last waypoint, `FollowPath` contributes no further force
(the boid doesn't loop back to the start automatically — set
`flock.Waypoints` again, or append the first point to the end of the list,
if you want a loop).

## Predator, prey, and VIP relationships

Pursuit/evasion/guard behaviors operate *between flocks*, not within one.
Each `Flock` has three lists of other flocks — `Predators`, `Prey`,
`Vips` — that its boids will react to. Use `AddFlockToGroup` to register
one flock's relationship to another:

```csharp
var deerFlock = new FlockManager(new Flock());
var wolfFlock = new FlockManager(new Flock());

// From the deer's perspective, the wolves are predators to evade:
deerFlock.Flock.AddFlockToGroup(wolfFlock.Flock, FlockGroup.Predator);
deerFlock.AddBehavior(BehaviorType.Evade);

// From the wolves' perspective, the deer are prey to pursue:
wolfFlock.Flock.AddFlockToGroup(deerFlock.Flock, FlockGroup.Prey);
wolfFlock.AddBehavior(BehaviorType.Pursuit);
```

The relationship is one-directional and you register it on the flock whose
boids should *react* — a flock only evades flocks it has added to its own
`Predators` list, regardless of whether the other flock considers it prey.
`AddFlockToGroup` removes the target flock from any other group on that
flock first, so a given flock pairing can only be one relationship
(`Predator`, `Prey`, or `Vip`) at a time. Use `IsFlockInGroup` to check the
current relationship, and `RemoveFlock` to clear it.

`Vip` relationships work the same way and back the guard behaviors
(`GuardSeparation`, `GuardAlignment`, `GuardCohesion`) as well as
`Interpose` and `OffsetPursuit`:

```csharp
guardFlock.Flock.AddFlockToGroup(vipFlockToProtect.Flock, FlockGroup.Vip);
guardFlock.AddBehavior(BehaviorType.GuardCohesion);
guardFlock.AddBehavior(BehaviorType.GuardSeparation);
```

In every case, only the *closest* boid in the related flock(s), within the
relevant query radius, is considered — see
[Configuration Reference](configuration-reference.md#query-radii).

## World wrap

By default, boids that leave `Flock.WorldSize` just keep going — FlockBuddy
doesn't clip or bounce them for you (use `WallAvoidance` with walls around
your play area if you want them contained). If you'd rather have a
toroidal world where boids re-appear on the opposite edge:

```csharp
flock.UseWorldWrap = true;
// WorldSize defaults to 1024x768 — set it to match your actual bounds
```

`WorldSize` isn't currently exposed as a settable property on `IFlock` /
`FlockManager` — if you need a different size, set it via a subclass of
`Flock` or open an issue/PR against the library, since as shipped it's a
private-set default.

## Cell-space partitioning for large flocks

By default, neighbor queries (`FindBoidsInRange`, closest
predator/prey/VIP, etc.) are brute-force `O(n)` scans over every boid in
the flock. For small-to-medium flocks this is fine. For large ones, assign
a `CellSpacePartition<IMover>` (from the companion
[CellSpacePartition](https://www.nuget.org/packages/CellSpacePartition)
package) to `Flock.CellSpace` and FlockBuddy will use it automatically for
every query — `Flock.UseCellSpace` just checks whether `CellSpace` is
non-null:

```csharp
using CellSpacePartitionLib;

flock.CellSpace = new CellSpacePartition<IMover>(
    new Vector2(worldWidth, worldHeight), numCellsX, numCellsY, maxEntitiesPerQuery);
```

Once assigned, `Flock.Update` keeps each boid's cell membership current
automatically, and you can render the grid for debugging with
`flock.DrawCells(primitive)`. Existing boids already added to the flock
before you assign `CellSpace` are **not** automatically added to it —
assign `CellSpace` before adding any boids. (Don't call
`flock.AddBoids(flock.Boids)` to backfill it after the fact — `AddBoids`
appends to the `Boids` list itself, so passing it its own list duplicates
every boid.) If you must add `CellSpace` after boids already exist, add
each one with `flock.CellSpace.Add(boid)` directly instead.

See the `CellSpacePartition` package's own documentation for constructor
parameters and tuning guidance (cell size should roughly match your typical
query radius).
