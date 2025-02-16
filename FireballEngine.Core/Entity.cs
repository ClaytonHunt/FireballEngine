namespace FireballEngine.Core;

public abstract class Entity 
{
    public Transform Transform { get; private set; } = new Transform();
    public CameraLayer CameraLayer { get; set; }

    public virtual Task LoadAsync() => Task.CompletedTask;
    public virtual void Update(float deltaMs) { }
    public virtual void Render(Renderer renderer, Camera camera) { }
}

