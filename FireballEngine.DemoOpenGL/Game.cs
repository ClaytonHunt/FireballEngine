using FireballEngine.Core;

namespace FireballEngine.DemoOpenGL;

public class Game : IGame
{
    public void Update(float deltaTime)
    {
        
    }

    public void Render(IFireballContext context)
    {
        context.Clear(Color.CornflowerBlue);
    }
}
