using FireballEngine.Core;
using FireballEngine.Core.Math;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;

namespace FireballEngine.DemoGame;

public class Game : IGame
{
    private Renderer? _renderer;
    private IInput? _input;
    private IAssetManager? _assetManager;
    private Material? _material;
    private float _rotation;
    private float _scale = 1.0f;
    private Vector2 _position = new Vector2(400, 300);
    private Texture2D? _playerTexture;

    public async Task OnLoad(IFireballContext context)
    {
        _input = context.Input;
        _renderer = context.CreateRenderer();
        _assetManager = context.AssetManager;

        var shader = context.CreateShader(ShaderType.Sprite);
        shader.Compile();
        _material = new Material(shader, Color.Red);

        await LoadAssets();
    }

    private async Task LoadAssets()
    {
        _playerTexture = await _assetManager!.Load<Texture2D>("/player/idle", "assets/sprites/ship_idle.png");
    }


    public async Task Update(float deltaMs)
    {
        float movementSpeed = 0.5f; // Adjust this value
        float timeStep = MathF.Min(deltaMs, 16.67f); // Clamping to 60 FPS max step

        // Movement
        if (_input!.IsKeyDown(KeyCode.W)) _position.Y += movementSpeed * timeStep;
        if (_input!.IsKeyDown(KeyCode.S)) _position.Y -= movementSpeed * timeStep;
        if (_input!.IsKeyDown(KeyCode.A)) _position.X -= movementSpeed * timeStep;
        if (_input!.IsKeyDown(KeyCode.D)) _position.X += movementSpeed * timeStep;

        // Rotation
        if (_input!.IsKeyDown(KeyCode.Right)) _rotation += 0.001f * timeStep;
        if (_input!.IsKeyDown(KeyCode.Left)) _rotation -= 0.001f * timeStep;

        // Scale
        if (_input!.IsKeyDown(KeyCode.Up)) _scale += 0.001f * timeStep;
        if (_input!.IsKeyDown(KeyCode.Down)) _scale -= 0.001f * timeStep;

        await Task.CompletedTask;
    }

    public async Task Render(IFireballContext context)
    {
        await context.Clear(Color.CornflowerBlue);       

        // if(_playerTexture == null || _material == null || _renderer == null) return;

        var modelMatrix = Matrix4.CreateScale(_scale) *
                          Matrix4.CreateRotationZ(_rotation) *
                          Matrix4.CreateTranslation(_position.X, _position.Y);
        
        float left = 0.0f, right = 800.0f;
        float bottom = 0.0f, top = 600.0f;
        float near = -1.0f, far = 1.0f;

        float[] projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);

        _material!.Use();
        _material!.Shader.SetMatrix("model", modelMatrix);                
        _material!.Shader.SetMatrix("view", Matrix4.Identity);
        _material!.Shader.SetMatrix("projection", projectionMatrix);        

        _renderer!.DrawSprite(_playerTexture!, _material!, 0, 0, 32, 50);

        await Task.CompletedTask;
    }
}