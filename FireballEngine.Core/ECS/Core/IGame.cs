namespace FireballEngine.Core.ECS.Core;

public interface IGame
{
    Task OnLoad(IFireballContext context);
    void Update(float deltaMs);
    void Render();
}
