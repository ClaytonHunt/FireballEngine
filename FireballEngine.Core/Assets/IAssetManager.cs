using System.Reflection;
using FireballEngine.Core.Utilities;

namespace FireballEngine.Core.Assets;

public interface IAsset {}
public interface IAssetManager
{
    /// <summary>
    /// Loads and stores an asset
    /// </summary>
    Task<T> Load<T>(string name, string path) where T : IAsset;

    /// <summary>
    /// Retrieves a stored asset
    /// </summary>
    T Get<T>(string name) where T : IAsset;
}

public abstract class AssetManager<TPlatformAsset> : IAssetManager where TPlatformAsset: IAsset
{
    protected readonly Dictionary<string, IAsset> _assets = new();
    protected readonly Dictionary<Type, Type> _registeredAssetTypes = new();

    public AssetManager()
    {
        RegisterAssetTypes();
    }

    protected void RegisterAssetTypes() 
    {
        Fire.Info("Registering asset types...");

         var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var assetTypes = assembly.GetTypes()
                .Where(t => typeof(TPlatformAsset).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).ToList();

            foreach (var assetType in assetTypes)
            {
                var genericContract = assetType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.IsAssignableTo(typeof(TPlatformAsset)));
                var baseAssetType = genericContract?.GetGenericArguments()[0];  

                if (baseAssetType != null)
                {
                    _registeredAssetTypes[baseAssetType] = assetType;
                    Fire.Info($"Registered asset type {baseAssetType.Name} with {assetType.Name}");
                }
                else
                {
                    Fire.Error($"Could not register asset type {assetType.Name}");
                }
            }
        }
    }

    public abstract Task<T> Load<T>(string name, string path) where T : IAsset;
    public abstract T Get<T>(string name) where T : IAsset;
}