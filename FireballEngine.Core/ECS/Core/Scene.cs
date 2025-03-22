using FireballEngine.Core.ECS.Components;
using FireballEngine.Core.Rendering;
using FireballEngine.Core.Utils;

namespace FireballEngine.Core.ECS.Core;

public class Scene
{
    private List<Entity> _entities = new();
    private Dictionary<CameraLayer, CameraComponent> _cameras = new();

    public void AddCamera(CameraLayer layer, CameraComponent camera)
    {
        if (_cameras.ContainsKey(layer))
        {
            Fire.Warning($"Camera layer {layer} already exists. Overwriting...");
        }

        _cameras[layer] = camera;
    }

    public void AddEntity(Entity entity)
    {
        if (!_cameras.ContainsKey(entity.CameraLayer))
        {
            Fire.Error($"Camera layer {entity.CameraLayer} does not exist.");
        }

        _entities.Add(entity);
    }

    public async Task LoadAsync()
    {
        foreach (var entity in _entities)
        {
            await entity.LoadAsync();
        }
    }

    public void Update(float deltaMs)
    {
        foreach(var camera in _cameras.Values)
        {
            camera.UpdateView();
        }

        foreach (var entity in _entities)
        {
            entity.Update(deltaMs);
        }
    }

    public void Render(Renderer renderer)
    {
        foreach (var entity in _entities)
        {
            if (_cameras.TryGetValue(entity.CameraLayer, out var camera))
            {                
                entity.Render(renderer, camera);
            }
            else
            {
                Fire.Error($"No camera found for layer '{entity.CameraLayer}'");
            }
        }
    }
}

