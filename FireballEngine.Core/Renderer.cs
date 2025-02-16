using FireballEngine.Core.Assets;

namespace FireballEngine.Core;

/// <summary>
/// Abstract renderer class for rendering shapes.
/// </summary>
public abstract class Renderer
{
    /// <summary>
    /// Draws a simple triangle
    /// </summary>
    public abstract void DrawTriangle();

    /// <summary>
    /// Draws a sprite with a texture and material
    /// </summary>
    public abstract void DrawSprite(Texture2D texture, Material material, float x, float y, float width, float height, SpriteOrigin origin = SpriteOrigin.Center, float customOriginX = 0.5f, float customOriginY = 0.5f);
}