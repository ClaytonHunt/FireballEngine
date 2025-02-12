namespace FireballEngine.Core;

public interface IGame
{
    void Update(float deltaMs);
    void Render(IFireballContext context);
}
