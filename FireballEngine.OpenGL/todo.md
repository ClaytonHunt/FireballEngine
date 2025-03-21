# FireballEngine.OpenGL TODO

This project implements the OpenGL-specific functionality of the engine.

## Directory Structure

- **Rendering**: OpenGL rendering pipeline
- **Shaders**: GLSL shaders and shader management
- **Input**: OpenGL/GLFW input handling
- **Components**: OpenGL-specific ECS components
- **Systems**: OpenGL-specific ECS systems
- **Platform**: Platform-specific code for OpenGL

## Implementation Tasks

1. Create OpenGL rendering pipeline:
   - Shader management
   - Texture loading
   - Mesh rendering
   - Batching and optimization

2. Implement GLFW input handling:
   - Map to the core input system
   - Handle window events

3. Create OpenGL-specific components:
   - `GLRendererComponent`
   - `MeshComponent`
   - `MaterialComponent`

4. Implement rendering systems:
   - `MeshRenderSystem`
   - `ParticleRenderSystem`
   - `UIRenderSystem`

5. Add platform-specific utilities:
   - Window management
   - OpenGL context handling
   - Error checking and validation
