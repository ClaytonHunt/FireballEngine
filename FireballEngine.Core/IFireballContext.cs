namespace FireballEngine.Core
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
        /// Asynchronously initializes with width, height, and title.
        /// </summary>
        Task InitializeAsync(int width, int height, string title);

        /// <summary>
        /// Factory method to create a platform-specific shader.
        /// </summary>
        Shader CreateShader(ShaderType type);

        /// <summary>
        /// Factory method to create a platform-specific renderer.
        /// </summary>
        Renderer CreateRenderer();

        /// <summary>
        /// Clears the screen with a color.
        /// </summary>
        Task Clear(Color color);
    }
}
