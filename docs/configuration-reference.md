# Configuration Reference

Every tunable value lives on `IBoid`/`Boid` and is mirrored on
`IFlockManager`/`FlockManager` as a `Boid<Name>` property. Setting a
property on the `FlockManager` immediately applies it to every existing
boid in its `Flock` and becomes the default for boids added afterward.
Defaults are defined in `FlockBuddy/BoidDefaults.cs`.

```csharp
flockManager.BoidMaxSpeed = 350f;      // applies to all current + future boids
someExistingBoid.MaxSpeed = 500f;      // override just one boid after the fact
```

## Movement

| Property | Default | Meaning |
|---|---|---|
| `Mass` | `1` | Divides the total combined steering force before it's applied — higher mass means the boid accelerates/turns more sluggishly for the same force. |
| `MinSpeed` | `140` | Floor on `Speed`; a boid never coasts slower than this once moving. |
| `WalkSpeed` | `200` | The "cruising" speed a boid settles toward when no behavior has a strong opinion — a boid speeds up toward this when it's below it and forces are weak, on the way to `MaxSpeed`. |
| `MaxSpeed` | `275` | Ceiling on `Speed`. Several behaviors (`Seek`, `Flee`, ...) compute their desired velocity at this speed. |
| `MaxForce` | `200` | The largest steering force magnitude a boid can apply in one update — clamps how sharply it can accelerate, and is the budget consumed by `SummingMethod.Prioritized`. |
| `MaxTurnRate` | `π` (≈3.1416 rad/s) | The maximum angle a boid can rotate its heading per second. |
| `Laziness` | `0.5` | If the combined steering force is smaller than this (squared-magnitude test), the boid won't bother accelerating up to `WalkSpeed` — keeps nearly-idle boids from twitchy speed changes. |
| `SummingMethod` | `WeightedAverage` | How multiple active behaviors' forces combine — see [Core Concepts](core-concepts.md#how-forces-combine-summingmethod). |

## Identity / collision

| Property | Default | Meaning |
|---|---|---|
| `Radius` (`BoidRadius` on the manager) | `10` | Bounding-circle radius, used for neighbor/obstacle distance checks and debug drawing. |
| `RetargetTime` | `0.1` (seconds) | How often a boid re-scans the flock for neighbors/predator/prey/VIP/obstacles. Lower = more responsive but more expensive; higher = cheaper but laggier reactions. |

## Query radii

Each of these bounds how far a boid looks for the corresponding thing when
it retargets. All default to `100` (`BoidDefaults.BoidQueryRadius`) unless
set individually.

| Property | Used by |
|---|---|
| `NeighborsQueryRadius` | Separation, Alignment, Cohesion |
| `PredatorsQueryRadius` | Evade (also doubles as Flee's "panic distance" check) |
| `PreyQueryRadius` | Pursuit |
| `VipQueryRadius` | GuardSeparation, GuardAlignment, GuardCohesion, Interpose, OffsetPursuit |
| `WallQueryRadius` | WallAvoidance (controls feeler length) |
| `ObstacleQueryRadius` | ObstacleAvoidance (detection box grows with current speed, up to double this at max speed) |
| `WaypointQueryRadius` | FollowPath ("close enough" distance to advance to the next waypoint) |

## Default behavior weights

Set automatically when you call `AddBehavior(BehaviorType)` without an
explicit weight; pass a second argument to override. See
[Steering Behaviors](behaviors.md) for what each one does.

| Behavior | Default weight |
|---|---|
| `WallAvoidance` | 50 |
| `ObstacleAvoidance` | 30 |
| `Separation` | 60 |
| `Alignment` | 10 |
| `Cohesion` | 1 |
| `Evade` | 1 |
| `Flee` | 1 |
| `Direction` | 1 |
| `Seek` | 1 |
| `Arrive` | 1 |
| `Wander` | 1 |
| `Pursuit` | 0.1 |
| `OffsetPursuit` | 1 |
| `Interpose` | 10 |
| `GuardSeparation` | 60 (shares `SeparationWeight`) |
| `GuardAlignment` | 10 (shares `AlignmentWeight`) |
| `GuardCohesion` | 1 (shares `CohesionWeight`) |
| `Hide` | 1 |
| `FollowPath` | 1 |

## World / boundary settings

These live on `Flock`/`FlockManager` rather than per-boid:

| Property | Default | Meaning |
|---|---|---|
| `Walls` (`DefaultWalls` enum: `None`, `All`, `TopBottom`, `LeftRight`) | `None` | Which edges `AddDefaultWalls` generates walls for — see [Advanced Topics](advanced-topics.md#walls). |
| `Flock.UseWorldWrap` | `false` | If `true`, boids that cross `Flock.WorldSize` teleport to the opposite edge (toroidal world) instead of being stopped/bounced. |
| `Flock.WorldSize` | `1024 x 768` | The bounds used for world-wrap. Set this to match your actual play area if you enable wrap. |

## Tuning tips

- Start from the defaults — they're the values the example project ships
  with and behave reasonably for a mid-sized flock at typical screen
  scales.
- If boids look "twitchy," raise `RetargetTime` slightly or lower
  `MaxTurnRate`.
- If boids overlap/clump, raise `Separation`'s weight relative to
  `Cohesion`'s.
- If a flock ignores walls/obstacles when flocking hard, either raise
  `WallAvoidance`/`ObstacleAvoidance` weight relative to the flocking
  weights, or switch `SummingMethod` to `Prioritized` so avoidance always
  gets first claim on `MaxForce`.
