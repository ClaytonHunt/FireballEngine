namespace FireballEngine.Core;

public interface IGame
{
    void OnLoad(IFireballContext context);
    void Update(float deltaMs);
    void Render(IFireballContext context);
}
