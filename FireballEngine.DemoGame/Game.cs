using FireballEngine.Core;
using FireballEngine.Core.Math;

namespace FireballEngine.DemoGame;

public class Game: IGame
{
    private Renderer? _renderer;
    private Material[] _materials = new Material[5];
    private float[] _rotations = new float[5];

    public void OnLoad(IFireballContext context)
    {
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
        _materials[0] = new Material(shader, new Color(1, 0, 0)); // Red
        _materials[1] = new Material(shader, new Color(0, 1, 0)); // Green
        _materials[2] = new Material(shader, new Color(0, 0, 1)); // Blue
        _materials[3] = new Material(shader, new Color(1, 1, 0)); // Yellow
        _materials[4] = new Material(shader, new Color(1, 0, 1)); // Magenta       
    }

    public void Update(float deltaMs)
    {
        for (int i = 0; i < _rotations.Length; i++)
            _rotations[i] += deltaMs * 0.001f; // Rotate over time
    }

    public void Render(IFireballContext context)
    {
        context.Clear(Color.CornflowerBlue);

        for (int i = 0; i < 5; i++)
        {
            float angle = _rotations[i];
            var modelMatrix = Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(i * 0.3f - 0.6f, 0);
            _renderer!.DrawTriangle(_materials[i], modelMatrix.ToArray());
        }
    }
}