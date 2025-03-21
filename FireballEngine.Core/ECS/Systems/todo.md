# Core Systems TODO

This folder contains the systems that process components in a platform-agnostic way.

## Systems to Implement

1. `TransformSystem`
   - Update world matrices
   - Handle parent-child relationships

2. `PhysicsSystem`
   - Integration with physics components
   - Collision detection and resolution

3. `BehaviorSystem`
   - Execute behavior component logic
   - Manage behavior lifecycle

4. `TimeSystem`
   - Handle engine time (delta time, fixed time step)
   - Manage timer components

5. `EventSystem`
   - Process and dispatch events between systems

## Integration Notes

These systems should be modular and composable. Each system should be focused on a specific task and not depend on other systems directly. Use events for cross-system communication.
