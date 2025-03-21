# Core Components TODO

This folder contains the basic components that are platform-agnostic.

## Components to Implement

1. `TransformComponent`
   - Position, rotation, scale
   - Parent-child relationships
   - World/Local space conversions

2. `HierarchyComponent`
   - Parent-child relationships between entities

3. `IdentityComponent`
   - Name, tags, layers

4. `BehaviorComponent`
   - Script-like functionality for entities

5. `TimerComponent`
   - Time-based events and callbacks

## Existing Code Changes

Current code should be refactored to use these components rather than inheritance-based game objects.
