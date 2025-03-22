using FireballEngine.Core.ECS.Components;
using FireballEngine.Core.Rendering;

namespace FireballEngine.Core.ECS.Core;

public abstract class Entity 
{
    public TransformComponent Transform { get; private set; } = new TransformComponent();
    public CameraLayer CameraLayer { get; set; }

    public virtual Task LoadAsync() => Task.CompletedTask;
    public virtual void Update(float deltaMs) { }
    public virtual void Render(Renderer renderer, CameraComponent camera) { }
}

