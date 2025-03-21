# Physics System TODO

This folder contains the physics simulation capabilities.

## Implementation Tasks

1. Create physics components:
   - `RigidbodyComponent`
   - `ColliderComponent` (with specific shape implementations)
   - `PhysicsMaterialComponent`

2. Implement the `PhysicsSystem`:
   - Force application
   - Collision detection
   - Constraint solving
   - Continuous collision detection

3. Add physics utility classes:
   - Raycasting
   - Overlap tests
   - Triggers and sensors

4. Create physics event system:
   - Collision begin/end events
   - Trigger enter/exit events

## Integration Notes

The physics system should:
- Work with the TransformSystem to update entity positions
- Support both 2D and 3D physics
- Allow for different levels of fidelity depending on game needs
- Consider using an existing physics library as a backend
