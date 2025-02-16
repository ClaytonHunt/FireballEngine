namespace FireballEngine.Core.Assets;

public abstract class Texture2D : IAsset
{
    public string Name { get; protected set; }
    public string Path { get; protected set; }

    protected Texture2D(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public abstract Task<bool> IsLoaded();
    public abstract void Bind();
    public abstract void Dispose();
}