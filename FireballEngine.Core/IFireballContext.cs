namespace FireballEngine.Core;

public delegate void UpdateHandler(float deltaTime);

/// <summary>
/// Main cross-platform context interface for the Fireball Engine.
/// </summary>
public interface IFireballContext
{
    /// <summary>
    /// Fires each frame for game logic, with deltaTime in miliseconds.
    /// </summary>
    event UpdateHandler OnUpdate;

    /// <summary>
    /// Fires each frame for rendering.
    /// </summary>
    event Action OnRender;

    /// <summary>
    /// Asyncronously initializes with width, height, and title.
    /// </summary>
    Task InitializeAsync(int width, int height, string title);

    /// <summary>
    /// Clears the screen with a color.
    /// </summary>
    /// <param name="color">The color to clear the screen with.</param>
    Task Clear(Color color);
}
