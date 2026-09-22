# Steering Behaviors

Every behavior is turned on with `flockManager.AddBehavior(BehaviorType.X)`
(or `boid.AddBehavior(...)` for a single boid) and produces a `Vector2`
steering force each time the boid retargets. This page documents what each
one does, what input it needs, and its default weight.

Where noted, a behavior is a **stub**: it's a real, selectable
`BehaviorType` with a working weight/priority slot, but `GetSteering()`
currently returns `Vector2.Zero` (there's a `// TODO:` in the source).
Adding a stubbed behavior to a flock is harmless — it just won't contribute
any force yet.

## Quick reference

| Behavior | Default weight | Needs | Status |
|---|---|---|---|
| [WallAvoidance](#wallavoidance) | 50 | `Flock.Walls` | Implemented |
| [ObstacleAvoidance](#obstacleavoidance) | 30 | nearby obstacles | Implemented |
| [Evade](#evade) | 1 | a predator | Implemented |
| [Flee](#flee) | 1 | a position to avoid | Implemented (internal-use; see below) |
| [Direction](#direction) | 1 | `SteeringDirection` set manually | Implemented |
| [Separation](#separation) | 60 | flock neighbors | Implemented |
| [Alignment](#alignment) | 10 | flock neighbors | Implemented |
| [Cohesion](#cohesion) | 1 | flock neighbors | Implemented |
| [Seek](#seek) | 1 | a target position set manually | Implemented (internal-use; see below) |
| [Arrive](#arrive) | 1 | a target position | **Stub** |
| [Wander](#wander) | 1 | — | **Stub** |
| [Pursuit](#pursuit) | 0.1 | prey | Implemented |
| [OffsetPursuit](#offsetpursuit) | 1 | a VIP to escort | **Stub** |
| [Interpose](#interpose) | 10 | a pursuer + a VIP | Implemented |
| [GuardSeparation](#guardseparation) | 60 | a VIP | Implemented |
| [GuardAlignment](#guardalignment) | 10 | a VIP | Implemented |
| [GuardCohesion](#guardcohesion) | 1 | a VIP | Implemented |
| [Hide](#hide) | 1 | a pursuer + obstacles | **Stub** |
| [FollowPath](#followpath) | 1 | `Flock.Waypoints` | Implemented, but buggy — see below |

Defaults live in `FlockBuddy/BoidDefaults.cs` and can always be overridden
per-behavior: `flockManager.AddBehavior(BehaviorType.Separation, 80f)`.

---

## Flocking behaviors

These three are the classic Craig Reynolds "boids" rules. All three look at
`Buddies` — the list of nearby flock-mates found by `Flock.FindBoidsInRange`
within `NeighborsQueryRadius` — which is refreshed automatically on each
boid's retarget cycle.

### Separation

Steers a boid away from nearby flock-mates so they don't pile on top of
each other. For every neighbor, it adds a vector pointing away from that
neighbor, scaled inversely by distance (closer neighbors push harder). This
is normally the strongest force in a flock (`SeparationWeight = 60`) since
without it boids visually collapse into each other.

### Alignment

Steers a boid to match the *average heading* of its neighbors, so the group
turns together instead of each boid facing a random direction. It only
affects direction, not speed (`SpeedChange = 0`).

### Cohesion

Steers a boid toward the *average position* (center of mass) of its
neighbors, via an internal `Seek`. This is what keeps a flock from drifting
apart into isolated boids. Its result is normalized before weighting, since
cohesion forces are naturally much larger in magnitude than
separation/alignment.

---

## Target-seeking behaviors

### Seek

Steers directly toward `TargetPosition` at `MaxSpeed`. `TargetPosition` is
private-set — you don't add `BehaviorType.Seek` and drive it yourself in
normal usage; it's the building block several other behaviors (`Cohesion`,
`FollowPath`, `Pursuit`, `Interpose`, `GuardCohesion`) create internally and
drive toward their own computed target each frame.

### Flee

The inverse of `Seek`: steers directly away from `AvoidPosition`, but only
if that position is within `PredatorsQueryRadius` ("panic distance") —
outside that range it contributes nothing. Like `Seek`, it's mainly a
building block (`Evade` uses it internally); its own `GetSteering(Vector2)`
overload lets you drive it directly with an arbitrary point if you do want
to use it standalone.

### Arrive — **stub**

Intended to behave like `Seek` but decelerate smoothly to a stop at the
target instead of overshooting/oscillating around it (the classic "arrival"
behavior from Reynolds' steering behaviors). Selectable and weighted, but
`GetSteering()` currently always returns zero force — do not rely on it to
slow a boid down yet; use `Seek`/`Cohesion`-style behaviors and rely on
`MinSpeed`/`MaxTurnRate` tuning in the meantime.

### Wander — **stub**

Intended to make a boid amble around with smoothly-varying random
direction changes (Reynolds' "wander circle" technique — the design comment
references using the boid's own `BoidTimer` for this) so idle boids don't
move in a perfectly straight line forever. Currently a stub returning zero
force. If you need idle motion today, use a very low-weight `Direction`
behavior with a periodically-randomized `SteeringDirection`, or generate
your own waypoints for `FollowPath`.

### Direction

The simplest possible behavior: steers toward whatever unit vector you set
on `SteeringDirection` yourself. Since this isn't populated automatically
from the flock, you set it directly on the behavior instance you get back
from `AddBehavior`:

```csharp
var direction = (Direction)flockManager.AddBehavior(BehaviorType.Direction, 1f);
direction.SteeringDirection = someUnitVector;
```

Useful for scripted movement (e.g. "always drift toward the wind") layered
on top of flocking.

---

## Pursuit and evasion

These use the flock's predator/prey relationships — see
[Advanced Topics](advanced-topics.md#predator-prey-and-vip-relationships)
for how to wire two flocks together so one hunts or flees the other.

### Pursuit

Chases the closest boid in the flocks registered as `Prey`, found within
`PreyQueryRadius`. Rather than just seeking the prey's *current* position
(which lags behind a moving target), it predicts where the prey will be and
seeks that instead — the lookahead time scales with distance and inversely
with the combined speed of predator and prey. If the prey is already ahead
of and facing roughly toward the pursuer (within ~18°), it seeks the
current position directly since prediction isn't needed. Set
`ViciousPursuit = true` on the behavior instance to always chase the
predicted position and skip that check. Default weight is deliberately low
(`0.1`) since pursuit is usually meant to bias movement, not dominate it.

### Evade

The mirror of `Pursuit`: runs from the closest boid in the flocks
registered as `Predators`, found within `PredatorsQueryRadius`. Predicts
the pursuer's future position the same way `Pursuit` does, then flees from
that predicted point (via an internal `Flee`).

### OffsetPursuit — **stub**

Intended to hold a fixed offset from a moving VIP — e.g. flying in
formation just behind and to the side of a lead boid — rather than
converging on its exact position the way `GuardCohesion` does. Selectable
and weighted, but currently returns zero force.

### Interpose

Intended to steer to the midpoint between a `Pursuer` and a `Vip`, so a
guard/escort boid places itself between an attacker and whoever it's
protecting: it computes the midpoint of pursuer and VIP, works out how long
it would take the guard to reach it, and predicts the pursuer's position at
that time. **Known bug:** the final seek target is computed as
`(predPos + predPos) / 2`, i.e. the predicted pursuer position averaged
with itself — the predicted VIP position (`preyPos`) is calculated in a
commented-out line and never actually used. In practice this makes the
behavior seek straight at the pursuer's predicted position rather than the
true pursuer/VIP midpoint. It still pulls a guard toward the threat, just
not exactly "interpose" as named — worth fixing in
`FlockBuddy/SteeringBehaviors/Interpose.cs` if you need the real midpoint
behavior.

### Hide — **stub**

Intended to put an obstacle between the boid and a `Pursuer` (classic
"hide behind cover" steering). Selectable and weighted (and already wired
up to receive both `Pursuer` and nearby `Obstacles` each retarget cycle),
but `GetSteering()` currently always returns zero force.

---

## Guard behaviors

These three mirror Separation/Alignment/Cohesion, but relative to a single
`Vip` (the closest flock member of a flock registered in `Vips`, found
within `VipQueryRadius`) instead of the average of all neighbors — use them
to have a flock escort/guard another flock as a group.

### GuardSeparation

Keeps the guard from crowding directly on top of its VIP — pushes away
from the VIP, scaled inversely by distance, the same way `Separation` does
for regular neighbors.

### GuardAlignment

Matches the guard's heading to the VIP's heading, the same way `Alignment`
matches the flock average.

### GuardCohesion

Seeks toward the VIP's position (via an internal `Seek`), the same way
`Cohesion` seeks toward the neighbor average — this is what actually pulls
a guard flock along with its VIP as it moves.

---

## Environment avoidance

### WallAvoidance

Casts three "feelers" (whiskers) from the boid — one straight ahead, one
angled left, one angled right, all scaled by `WallQueryRadius` — and tests
each against every line segment in `Flock.Walls`. If a feeler crosses a
wall, it adds a force in the wall's normal direction, scaled by how far the
feeler overshoots the wall. This is the highest-priority behavior by
default (`WallAvoidanceWeight = 50`, and first in `Prioritized` order) so
boids reliably stay inside bounds even while flocking. See
[Advanced Topics](advanced-topics.md#walls) for how to create walls.
`Boid.DrawWallFeelers` / `Flock.DrawWhiskers` can render the feeler lines
for debugging.

### ObstacleAvoidance

Looks at every entity in range (from `Flock.FindObstaclesInRange`, using a
detection box that grows with the boid's current speed) and, for the single
closest one that's ahead of the boid and intersects its path, steers away
from it — the closer the obstacle, the stronger the push. Default weight
(`30`) sits between wall avoidance and flocking, so boids dodge scenery
without ignoring the group. Obstacles are anything implementing
`IBaseEntity` (position + radius) added to `Flock.Obstacles` — they don't
have to be boids.

### FollowPath

Intended to walk a boid through `Flock.Waypoints` in order: seek the
current waypoint until it's within `WaypointQueryRadius`, then advance to
the next one. **Known bug:** the advance check is inverted — it moves to
the next waypoint whenever it is **not** yet within `WaypointQueryRadius`
of the current one (`FlockBuddy/SteeringBehaviors/FollowPath.cs`, the `>=`
should be `<`), and this check runs every frame, not just on the retarget
cycle. In practice that means a boid starting more than
`WaypointQueryRadius` away from waypoint 0 (the normal case) advances its
target waypoint on essentially every frame regardless of whether it's
actually gotten there, racing through the whole list in a handful of
frames and then producing zero force — it does not reliably visit any
waypoint in the middle of the path. Until this is fixed upstream, don't
rely on `FollowPath` to visit intermediate waypoints; a single-target
`Seek` you re-aim yourself once each leg is reached is a safer substitute.
See [Advanced Topics](advanced-topics.md#waypoint-paths) for how paths are
wired up regardless.

---

## Notes on stub behaviors

`Arrive`, `Wander`, `OffsetPursuit`, and `Hide` are part of the public
`BehaviorType` enum and fully wired into the priority/weight/factory
machinery — they just don't produce a force yet in this version of the
library. They're safe to add (they're a no-op) but shouldn't be relied on
for the effect their name implies. If you need one of these specifically,
check `FlockBuddy/SteeringBehaviors/<Name>.cs` for the current
`// TODO:` before building on top of it, since this is exactly the kind of
gap that tends to get filled in over time.
