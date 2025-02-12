using FireballEngine.Core;
using FireballEngine.OpenGL;

IFireballContext context = new FireballContextOpenGL();

context.OnUpdate += (deltaTime) =>
{
    
};

context.OnRender += () =>
{
    context.Clear(Color.CornflowerBlue);
};

await context.InitializeAsync(800, 600, "Fireball Engine Demo");