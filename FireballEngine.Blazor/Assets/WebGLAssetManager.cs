using System.Reflection;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor.Assets;

public class WebGLAssetManager : AssetManager<IWebGLAsset>
{
    private IJSObjectReference _jsModule;

    public WebGLAssetManager(IJSObjectReference jsModule)
    {
        _jsModule = jsModule;
    }

    public override async Task<T> Load<T>(string name, string path)
    {
        if (_registeredAssetTypes.TryGetValue(typeof(T), out var assetType))
        {
            var method = assetType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
            if (method != null)
            {
                var task = (Task<T>)method.Invoke(null, [_jsModule, name, path])!;
                var asset = await task;
                _assets[name] = (IWebGLAsset<T>)asset;

                return asset;
            }
            
            Fire.Error($"Asset type {typeof(T).Name} does not have a Load method.");
            throw new Exception($"[Fireball] Asset type {typeof(T).Name} does not have a Load method.");
        }

        Fire.Error($"Asset type {typeof(T).Name} not supported.");
        throw new Exception($"[Fireball] Asset type {typeof(T).Name} not supported.");
    }

    public async Task<bool> IsTextureLoaded(string name)
    {
        return await _jsModule.InvokeAsync<bool>("isTextureLoaded", name);
    }

    public override T Get<T>(string name)
    {
        if (_assets.TryGetValue(name, out var asset) && asset is T typedAsset)
        {
            return typedAsset;
        }

        throw new Exception($"Asset '{name}' not found.");
    }
}