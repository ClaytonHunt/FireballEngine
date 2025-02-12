using FireballEngine.Core;

namespace FireballEngine.DemoBlazor.Components;

public class Game: IGame
{
    public void Update(float deltaMs)
    {
        // My game logic
    }

    public void Render(IFireballContext context)
    {
        context.Clear(Color.CornflowerBlue);
        // My rendering code
    }
}