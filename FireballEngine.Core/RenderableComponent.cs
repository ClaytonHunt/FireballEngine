namespace FireballEngine.Core;

public abstract class RenderableComponent
{
    public Material Material { get; }

    protected RenderableComponent(Material material)
    {
        Material = material;
    }

    public abstract void Render(Renderer renderer, Camera camera, Transform transform);
}

