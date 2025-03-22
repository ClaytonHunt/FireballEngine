using FireballEngine.Core.Rendering;

namespace FireballEngine.Core.ECS.Systems
{
    /// <summary>
    /// Interface for systems that handle rendering.
    /// </summary>
    public interface IRenderSystem : ISystem
    {
        /// <summary>
        /// Renders using the provided renderer.
        /// </summary>
        void Render(Renderer renderer);
    }
}
