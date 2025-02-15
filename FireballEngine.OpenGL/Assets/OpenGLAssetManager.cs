using System.Drawing;
using System.Reflection;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;

namespace FireballEngine.OpenGL.Assets;

public class OpenGLAssetManager : AssetManager<IOpenGLAsset>
{
    public override async Task<T> Load<T>(string name, string path)
    {
        FireballMessage.Info($"Loading asset {name} from {path}");

        if (_registeredAssetTypes.TryGetValue(typeof(T), out var assetType))
        {
            var method = assetType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
            if (method != null)
            {
                var task = (Task<T>)method.Invoke(null, [name, path])!;
                var asset = await task;
                _assets[name] = (IOpenGLAsset<T>)asset;

                return asset;
            }
            
            FireballMessage.Error($"Asset type {typeof(T).Name} does not have a Load method.");
            throw new Exception($"[Fireball] Asset type {typeof(T).Name} does not have a Load method.");

        }

        FireballMessage.Error($"Asset type {typeof(T).Name} not supported.");
        throw new Exception($"[Fireball] Asset type {typeof(T).Name} not supported.");
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
