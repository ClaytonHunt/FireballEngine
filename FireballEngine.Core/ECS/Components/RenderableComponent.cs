using FireballEngine.Core.Assets;
using FireballEngine.Core.Rendering;

namespace FireballEngine.Core.ECS.Components;

public abstract class RenderableComponent
{
    public Material Material { get; }

    protected RenderableComponent(Material material)
    {
        Material = material;
    }

    public abstract void Render(Renderer renderer, CameraComponent camera, TransformComponent transform);
}

