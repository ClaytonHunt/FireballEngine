# FireballEngine.Core TODO

This is the core, client-agnostic part of the engine. It should contain all the fundamental systems that don't depend on specific rendering implementations.

## Directory Structure Overview

- **ECS**: Contains the Entity Component System implementation
  - **Core**: Base classes for the ECS system (Entity, Component, System)
  - **Components**: Common, platform-agnostic components
  - **Systems**: Common, platform-agnostic systems
- **Assets**: Asset loading and management
- **Input**: Abstract input systems
- **Math**: Math utilities and structures
- **Physics**: Physics simulation
- **Serialization**: Data saving/loading utilities
- **Utils**: General utility classes

## Implementation Plan

1. Implement base ECS framework
2. Move existing code to appropriate folders
3. Create abstractions for platform-specific features
4. Implement core game loop and update systems
