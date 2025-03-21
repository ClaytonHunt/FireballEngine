# ECS System TODO

The ECS (Entity Component System) is the backbone of the engine architecture.

## Implementation Tasks

1. Create base `Entity` class
   - Unique ID generation
   - Component management (add, remove, get)
   - Enable/disable functionality
   
2. Create base `Component` abstract class
   - Lifecycle methods (Initialize, Update, Destroy)
   - Reference to parent entity
   
3. Create `System` abstract class
   - Query for entities with specific component combinations
   - Process components in an optimized way
   
4. Create `World` or `Scene` class
   - Container for entities
   - System registration and execution order
   - Game loop integration

5. Implement event system for ECS communication
