# ECS Core Components TODO

This folder contains the foundational classes for the Entity Component System architecture.

## Implementation Tasks

1. Refactor the existing `Entity` class
   - Remove inheritance in favor of composition
   - Add component management functionality
   - Implement a unique ID system for entities

2. Create `Component` base class
   - Lifecycle methods (Initialize, Update, OnEnable, OnDisable, OnDestroy)
   - Entity reference handling
   - Enable/disable functionality

3. Create the `System` base class
   - Query for entities with specific component combinations
   - Implement update methods for processing components
   - Handle system priority/execution order

4. Implement `World` class
   - Entity container/management
   - System registration and execution
   - Scene management integration

5. Refactor `Transform` to be a component
   - Move from a standalone class to a component
   - Maintain backward compatibility where needed
   - Add parent-child hierarchy support

6. Create an `EntityManager` class
   - Factory methods for entity creation
   - Entity pooling for performance
   - Deferred entity destruction

## Migration Strategy

1. First, create the new ECS architecture alongside the existing classes
2. Add compatibility layers to ease transition
3. Move functionality incrementally from old to new architecture
4. Update existing game code to use the new component-based approach

## Design Principles

- Favor composition over inheritance
- Aim for cache-friendly data layouts where possible
- Keep systems focused on a single responsibility
- Design for extensibility and modularity
