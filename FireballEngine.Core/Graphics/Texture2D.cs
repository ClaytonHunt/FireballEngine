namespace FireballEngine.Core.Graphics;

public class Texture2D : ITexture
{
    public int Width { get; }
    public int Height { get; }

    protected readonly int _textureId;

    public Texture2D(int width, int height, int textureId)
    {
        Width = width;
        Height = height;
        _textureId = textureId;
    }

    public virtual void Bind()
    {
        // This method will be implemented in the platform-specific projects
    }
}