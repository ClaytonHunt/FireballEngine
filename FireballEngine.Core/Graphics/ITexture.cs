namespace FireballEngine.Core.Graphics;

public interface ITexture
{
    int Width { get; }
    int Height { get; }
    void Bind();
}