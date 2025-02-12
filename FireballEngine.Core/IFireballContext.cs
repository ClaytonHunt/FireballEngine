namespace FireballEngine.Core
{
    public interface IFireballContext 
    {
        /// <summary>
        /// The game instance that this context drives.
        /// </summary>
        IGame Game { get; }

        /// <summary>
        /// Asynchronously initializes with width, height, and title.
        /// </summary>
        Task InitializeAsync(int width, int height, string title);

        /// <summary>
        /// Clears the screen with a color.
        /// </summary>
        Task Clear(Color color);
    }
}
