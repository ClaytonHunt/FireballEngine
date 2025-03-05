using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;

namespace FireballEngine.Core;

public class Sprite : RenderableComponent
{
    public Texture2D Texture { get; }
    public float Width { get; }
    public float Height { get; }
    public SpriteOrigin Origin { get; set; } = SpriteOrigin.Center;
    public float CustomOriginX { get; set; } = 0.5f;
    public float CustomOriginY { get; set; } = 0.5f;

    public Sprite(Material material, Texture2D texture, float width, float height, 
        SpriteOrigin origin = SpriteOrigin.Center, float customOriginX = 0.5f, float customOriginY = 0.5f)
        : base(material)
    {
        Texture = texture;
        Width = width;
        Height = height;
        Origin = origin;
        CustomOriginX = customOriginX;
        CustomOriginY = customOriginY;
    }

    public override void Render(Renderer renderer, Camera camera, Transform transform)
    {
        Material.Use();
        Material.Shader.SetMatrix("model", transform.GetTransformationMatrix());
        Material.Shader.SetMatrix("view", camera.ViewMatrix);
        Material.Shader.SetMatrix("projection", camera.ProjectionMatrix);        

        renderer.DrawSprite(Texture, Material, 
            0, 0, 
            Width, Height,
            Origin, CustomOriginX, CustomOriginY);
    }
}

