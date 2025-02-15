using FireballEngine.Core.Assets;

namespace FireballEngine.OpenGL.Assets;

public interface IOpenGLAsset : IAsset { }

public interface IOpenGLAsset<T> : IOpenGLAsset where T : IAsset 
{
    public abstract static Task<T> Load(string name, string path);
}
