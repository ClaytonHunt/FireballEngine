using FireballEngine.Core.Graphics;

namespace FireballEngine.Core
{
    /// <summary>
    /// Abstract renderer class for rendering shapes.
    /// </summary>
    public abstract class Renderer
    {
        /// <summary>
        /// Draws a simple triangle
        /// </summary>
        public abstract void DrawTriangle(Material material, float[] modelMatrix);
    }
}