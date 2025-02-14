using FireballEngine.DemoGame;
using FireballEngine.OpenGL;

var game = new Game();
var context = new FireballContextOpenGL(game);

await context.InitializeAsync(800, 600, "Fireball Engine OpenGL Demo");