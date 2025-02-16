namespace FireballEngine.Core;

public interface IGame
{
    Task OnLoad(IFireballContext context);
    Task Update(float deltaMs);
    Task Render(IFireballContext context);
}
