using FireballEngine.Core;
using FireballEngine.Core.Math;

namespace FireballEngine.DemoGame;

public class Game: IGame
{
    private Renderer? _renderer;
    private IInput? _input;
    private Material? _material;
    private float _rotation;
    private float _scale = 1.0f;
    private Vector2 _position = Vector2.Zero;

    public void OnLoad(IFireballContext context)
    {
        _input = context.Input;
        _renderer = context.CreateRenderer();
        var shader = context.CreateShader(@"#version 300 es
            precision mediump float;
            layout (location = 0) in vec3 aPos;
            uniform mat4 model;
            void main() { 
                gl_Position = model * vec4(aPos, 1.0);
            }
        ", @"#version 300 es
            precision mediump float;
            out vec4 FragColor;
            uniform vec4 uColor;
            void main() { FragColor = uColor; }
        ");

         shader.Compile();

        // Create materials
        _material = new Material(shader, Color.Red);
    }

    public void Update(float deltaMs)
    {
        float movementSpeed = 0.005f; // Adjust this value
        float timeStep = MathF.Min(deltaMs, 16.67f); // Clamping to 60 FPS max step

        // Movement
        if (_input!.IsKeyDown(KeyCode.W)) _position.Y += movementSpeed * timeStep;
        if (_input!.IsKeyDown(KeyCode.S)) _position.Y -= movementSpeed * timeStep;
        if (_input!.IsKeyDown(KeyCode.A)) _position.X -= movementSpeed * timeStep;
        if (_input!.IsKeyDown(KeyCode.D)) _position.X += movementSpeed * timeStep;

        // Rotation
        if(_input!.IsKeyDown(KeyCode.Right)) _rotation += 0.001f * timeStep;
        if(_input!.IsKeyDown(KeyCode.Left)) _rotation -= 0.001f * timeStep;

        // Scale
        if(_input!.IsKeyDown(KeyCode.Up)) _scale += 0.001f * timeStep;
        if(_input!.IsKeyDown(KeyCode.Down)) _scale -= 0.001f * timeStep;
    }

    public void Render(IFireballContext context)
    {
        context.Clear(Color.CornflowerBlue);

        var modelMatrix = Matrix4.CreateScale(_scale) *
                  Matrix4.CreateRotationZ(_rotation) *
                  Matrix4.CreateTranslation(_position.X, _position.Y);
        
        _renderer!.DrawTriangle(_material!, modelMatrix);
    }
}