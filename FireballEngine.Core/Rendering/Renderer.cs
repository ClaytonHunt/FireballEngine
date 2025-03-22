using FireballEngine.Core.Assets;
using FireballEngine.Core.Utils;

namespace FireballEngine.Core.Rendering
{
    /// <summary>
    /// Abstract base class for all renderers. Platform-specific implementations 
    /// will derive from this to provide concrete rendering capabilities.
    /// </summary>
    public abstract class Renderer
    {
        /// <summary>
        /// Clears the screen with the specified color.
        /// </summary>
        /// <param name="color">The color to clear the screen with.</param>
        public abstract void Clear(Color color);

        /// <summary>
        /// Draws a simple triangle. Mainly used for testing.
        /// </summary>
        public abstract void DrawTriangle();

        /// <summary>
        /// Draws a sprite with the specified parameters.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="material">The material to use for rendering.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        /// <param name="origin">The origin point of the sprite.</param>
        /// <param name="customOriginX">Custom x origin (0-1) if using SpriteOrigin.Custom.</param>
        /// <param name="customOriginY">Custom y origin (0-1) if using SpriteOrigin.Custom.</param>
        public abstract void DrawSprite(
            Texture2D texture, 
            Material material, 
            float x, float y, 
            float width, float height, 
            SpriteOrigin origin = SpriteOrigin.Center, 
            float customOriginX = 0.5f, 
            float customOriginY = 0.5f);
    }
}
