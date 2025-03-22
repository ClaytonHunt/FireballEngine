using FireballEngine.Core.Assets;
using FireballEngine.Core.Input;
using FireballEngine.Core.Rendering;

namespace FireballEngine.Core.ECS.Core
{
    public interface IFireballContext
    {
        /// <summary>
        /// The game instance that this context drives.
        /// </summary>
        IGame Game { get; }

        /// <summary>
        /// The input system for this context.
        /// </summary>
        IInput Input { get; }

        /// <summary>
        /// Factory method to create a platform-specific asset manager.
        /// </summary>
        IAssetManager AssetManager { get; }

        Renderer Renderer { get; }

        /// <summary>
        /// Asynchronously initializes with width, height, and title.
        /// </summary>
        Task InitializeAsync(int width, int height, string title);

        /// <summary>
        /// Factory method to create a platform-specific shader.
        /// </summary>
        Shader CreateShader(ShaderType type);
    }
}
