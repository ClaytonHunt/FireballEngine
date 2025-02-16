using FireballEngine.Core.Assets;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor.Assets;

public interface IWebGLAsset : IAsset { }

public interface IWebGLAsset<T> : IWebGLAsset where T : IAsset 
{
    public abstract static Task<T> Load(IJSObjectReference jsModule, string name, string path);
}
