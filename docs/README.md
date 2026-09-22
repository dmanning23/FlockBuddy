# FlockBuddy Documentation

- **[Getting Started](getting-started.md)** — install FlockBuddy and build a
  working flock step by step, from a bare `Flock`/`FlockManager` pair
  through behaviors, boids, walls, and the update/draw loop.
- **[Core Concepts](core-concepts.md)** — the object model
  (`BaseEntity` → `Mover` → `Boid`, `Flock` vs. `FlockManager`) and how a
  boid turns its active behaviors into one steering force each frame.
- **[Steering Behaviors](behaviors.md)** — what every behavior does, what
  input it needs, its default weight, and which ones are still stubs.
- **[Configuration Reference](configuration-reference.md)** — every tunable
  boid parameter, its default, and what it controls.
- **[Advanced Topics](advanced-topics.md)** — walls, obstacles, waypoint
  paths, predator/prey/VIP relationships between flocks, toroidal world
  wrap, and cell-space partitioning for large flocks.

See the top-level [README](../README.md) for installation and a quick-start
snippet.
