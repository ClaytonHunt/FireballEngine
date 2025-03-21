# Assets System TODO

This folder handles asset loading, caching, and management.

## Implementation Tasks

1. Create `AssetManager` class
   - Asset loading by path or ID
   - Asset caching
   - Hot-reloading support

2. Implement abstract asset types:
   - `Texture`
   - `Model`
   - `Sound`
   - `Material`
   - `Shader`

3. Create asset loading pipeline
   - Asynchronous loading
   - Progress tracking
   - Error handling

4. Implement asset serialization system
   - Common format for storing metadata
   - Platform-specific loading handled by derived classes

## Integration with ECS

Create relevant components that reference assets, such as:
- `TextureComponent`
- `ModelComponent`
- `AudioComponent`
