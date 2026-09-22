# Core Concepts

This page explains the object model: what each class is responsible for,
and how a steering force is produced each frame.

## Class overview

```
BaseEntity            (position, radius, draws a circle)
  └─ Mover             (+ heading, speed, velocity)
       └─ Boid          (+ behaviors, tuning, the per-frame steering pipeline)

Flock                 (owns the boids + world data: walls, obstacles, waypoints,
                        predator/prey/VIP relationships, optional cell-space grid)

FlockManager           (owns tuning defaults + the active behavior list;
                        factory for creating boids that already match that config)

BaseBehavior           (abstract base for every steering behavior)
  └─ Seek, Flee, Arrive, Wander, Pursuit, Evade, OffsetPursuit, Interpose,
     Separation, Alignment, Cohesion, ObstacleAvoidance, WallAvoidance,
     FollowPath, Hide, GuardSeparation, GuardAlignment, GuardCohesion, Direction
```

### `BaseEntity`

The root of the hierarchy. Just a `Position`, `Radius`, and an
`Update(GameClock)`/`DrawPhysics` pair. Implements `IBaseEntity`, which is
also the type used for generic obstacles (a `Flock`'s `Obstacles` list is
`List<IBaseEntity>`, so anything with a position and radius can be an
obstacle, not just a `Boid`).

### `Mover`

Adds `Heading` (a unit vector), `Speed`, and the derived `Velocity`
(`Heading * Speed`). Also owns a `GameClock` used for frame-independent
timing, and helpers for rotating the heading vector by a clamped angle each
frame (`RotateHeading`).

### `Boid`

The actual flocking agent (`Mover` + `IBoid`). Each frame, `Boid.Update`:

1. Advances its internal clock.
2. If its **retarget timer** (`RetargetTime`, default every 0.1s — see
   [Configuration Reference](configuration-reference.md)) has elapsed,
   re-queries the `Flock` for nearby neighbors, the closest predator/prey/VIP
   in range, nearby obstacles, and hands those lists to whichever active
   behaviors need them (see [Steering Behaviors](behaviors.md) for which
   behavior wants which inputs). Queries are throttled by the retarget timer
   because scanning the flock every single frame for every boid is
   expensive — increase `RetargetTime` for cheaper/laggier reactions,
   decrease it for snappier/costlier ones.
3. Asks every active behavior for its steering vector and combines them
   into a single force (see **Summing methods**, below).
4. Uses that force to update `Speed` (clamped to `MinSpeed`/`MaxSpeed`) and
   `Heading` (turned by at most `MaxTurnRate` radians/sec), then advances
   `Position` by `Velocity * TimeDelta`.
5. Asks the `Flock` to wrap the new position if `UseWorldWrap` is on.

A `Boid` doesn't talk to other boids directly — it only ever asks its owning
`Flock` "who's near me," so all cross-boid logic (and any performance
optimization like cell-space partitioning) lives on `Flock`, not `Boid`.

### `Flock`

Holds `Boids` (the actual agent list), `Obstacles`, `Walls`, `Waypoints`,
and three lists of *other flocks* — `Predators`, `Prey`, `Vips` — used by
predator/prey/guard behaviors (see
[Advanced Topics](advanced-topics.md#predator-prey-and-vip-relationships)).
It also owns the neighbor-query logic (`FindBoidsInRange`,
`FindClosestPredatorInRange`, etc.), either by brute-force distance checks
or, if you assign a `CellSpace`, via spatial partitioning (see
[Advanced Topics](advanced-topics.md#cell-space-partitioning-for-large-flocks)).

`Flock.Update(GameClock)` just loops over `Boids` and calls `Boid.Update` on
each one, then refreshes the cell-space grid if one is in use.

### `FlockManager`

The configuration/factory layer. It mirrors every tunable `Boid` property
(`BoidMass`, `BoidMaxSpeed`, `BoidNeighborQueryRadius`, ...) as its own
property; setting one of these on the manager immediately pushes the new
value onto every existing boid in its `Flock`, and new boids created via
`AddBoid` pick up whatever the manager's current values are. It also owns
the list of active `BehaviorTemplate`s (type + weight pairs) that get
applied to every boid it creates.

Because it's a thin wrapper around `Flock`, you can construct a
`FlockManager` either around a fresh `Flock` (`new FlockManager(flock)`) or
by cloning another manager's settings onto a new flock
(`new FlockManager(otherManager)` — copies every tuning value and wall
setting, but not the boids or behavior list).

> **Note:** `IFlockManager` (the interface, used for mocking in tests) only
> declares the tuning *properties* — `AddBoid`, `AddBehavior`,
> `RemoveBehavior`, `SetBehaviorWeight`, `AddDefaultWalls`, `HasBehavior`,
> `GetAllBehaviors`, and `GetBehaviorWeight` all live on the concrete
> `FlockManager` class only. Declare your variables as `FlockManager`, not
> `IFlockManager`, unless you specifically only need the tuning properties.

### Behaviors (`IBehavior` / `BaseBehavior`)

Each behavior is a small, focused class: given whatever inputs it needs
(a target position, a list of neighbors, a pursuer, obstacles, walls...) it
computes a single `Vector2` steering force in `GetSteering()`. Behaviors are
created via `BaseBehavior.BehaviorFactory(BehaviorType, IBoid)` — you never
`new` one up directly, you go through `FlockManager.AddBehavior` /
`Boid.AddBehavior`.

Several behaviors are implemented on top of a private `Seek` (or `Flee`)
instance internally — e.g. `Cohesion` seeks toward the neighbors' average
position, `Pursuit` seeks toward a predicted future position of its prey,
`FollowPath` seeks toward the current waypoint. This keeps the "move toward
a point" math in one place.

Every behavior also reports two multipliers, `DirectionChange` and
`SpeedChange` (both typically `1`, sometimes `0` or `0.5`), that scale how
much of that behavior's force affects heading versus speed when the boid
combines it with the others — e.g. `Alignment`'s `SpeedChange` is `0`
because matching a neighbor's heading shouldn't change your speed.

## How forces combine: `SummingMethod`

A boid can have several behaviors active at once. `SummingMethod` (set on
the `FlockManager`/`Boid`) controls how their individual steering vectors
combine into the one force actually applied:

| Method | Behavior |
|---|---|
| `WeightedAverage` (default) | Every active behavior's steering vector is multiplied by its weight and summed together, then divided by `Mass`. Simple and smooth, but a boid can be pulled by many competing forces at once and none of them "wins" outright. |
| `Prioritized` | Behaviors are evaluated in `BehaviorType` enum order (see below) and their weighted forces are accumulated *until the boid's `MaxForce` budget is used up* — once the budget runs out, lower-priority behaviors contribute nothing that frame. This is how you guarantee, e.g., wall avoidance always wins over wandering. |
| `Dithered` | Present as an enum value and selectable, but **not implemented** in this version of `Boid` — `GetForces` falls through to it as the `default` case, and the method currently returns zero force for every behavior. Avoid using it until it's implemented. |

Behavior priority for `Prioritized` mode follows the declaration order of
the `BehaviorType` enum (`FlockBuddy/BehaviorType.cs`), from highest to
lowest priority:

```
WallAvoidance, ObstacleAvoidance, Evade, Flee, Direction, Separation,
Alignment, Cohesion, Seek, Arrive, Wander, Pursuit, OffsetPursuit,
Interpose, GuardSeparation, GuardAlignment, GuardCohesion, Hide, FollowPath
```

`Boid.AddBehavior` keeps its internal behavior list sorted by this order
automatically, so you don't need to add behaviors in any particular
sequence yourself.

## Reading boid state for rendering

`FlockManager.AddBoid` returns an `IBoid`, and `Flock.Boids` is a
`List<IMover>` — both expose the state you need to draw your own sprites
instead of (or alongside) the debug primitives:

- `Position` — world position (`Vector2`)
- `Heading` — unit facing vector
- `Rotation` — `Heading` as a radian angle, ready for `SpriteBatch.Draw`'s
  `rotation` parameter
- `Speed` / `Velocity`
- `Radius`

## Data-only defaults: `BehaviorTemplate`

`BehaviorTemplate` is a lightweight `IBehavior` implementation that only
carries a `BehaviorType` and `Weight` — its `GetSteering()` and related
members throw `NotImplementedException`. It's what `FlockManager.Behaviors`
actually stores; it exists purely to remember "this flock should have
Separation at weight 60" without needing a live `Boid` to attach the real
behavior instance to.
