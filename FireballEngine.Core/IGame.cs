namespace FireballEngine.Core;

public interface IGame
{
    Task OnLoad(IFireballContext context);
    void Update(float deltaMs);
    void Render();
}
