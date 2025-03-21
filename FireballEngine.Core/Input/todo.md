# Input System TODO

This folder contains input handling that is platform-agnostic.

## Implementation Tasks

1. Create `InputManager` abstract class
   - Define common input events and state tracking
   - Platform-specific implementations will derive from this

2. Implement input mapping system
   - Map logical actions to physical inputs
   - Support for rebinding controls

3. Create input event system
   - Key/button pressed/released events
   - Pointer/touch events
   - Gamepad support

4. Input component for ECS
   - Entity-specific input handling
   - Integration with behavior components

## Integration Notes

The input system should be abstracted enough that platform-specific implementations (Blazor, OpenGL) can provide the concrete details, while game code uses the same API regardless of platform.
