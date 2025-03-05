using FireballEngine.Core;
using FireballEngine.Core.Math;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;

namespace FireballEngine.DemoGame;

public class Game : IGame
{
    private Scene? _scene;
    private IInput? _input;
    private IAssetManager? _assetManager;
    private Renderer? _renderer;    

    public async Task OnLoad(IFireballContext context)
    {
        _input = context.Input;        
        _assetManager = context.AssetManager;
        _renderer = context.Renderer;
        _scene = new Scene();

        // Add Camera
        var camera = new Camera(ProjectionType.Orthographic, 800, 600);
        _scene.AddCamera(CameraLayer.Game, camera);

        // Add Player
        var player = new Player(context, _input, _assetManager);
        _scene.AddEntity(player);        

        // Add Asteroid
        var asteroid = new Asteroid(context, _input, _assetManager);
        _scene.AddEntity(asteroid);

        await _scene.LoadAsync();
    }

    public void Update(float deltaMs)
    {
        _scene!.Update(deltaMs);
    }

    public void Render()
    {
        _renderer!.Clear(Color.CornflowerBlue);
        _scene!.Render(_renderer!);
    }
}

public class Player : Entity
{
    private IFireballContext _context;
    private IInput _input;
    private IAssetManager _assetManager;
    private Sprite _sprite;
    private float _speed = .5f;

    public Player(IFireballContext context, IInput input, IAssetManager assetManager)
    {
        _context = context;
        _input = input;        
        _assetManager = assetManager;
        CameraLayer = CameraLayer.Game;
    }

    public override async Task LoadAsync()
    {
        // Load assets
        var shader = _context.CreateShader(ShaderType.Sprite);
        shader.Compile();

        var material = new Material(shader, Color.White);
        var texture = await _assetManager.Load<Texture2D>("/player/idle", "assets/sprites/ship_idle.png");

        float spriteWidth = 32;
        float spriteHeight = 50;

        Transform.Position = new Vector3(400, 300, 0);

        _sprite = new Sprite(material, texture, spriteWidth, spriteHeight);
    }

    public override void Update(float deltaMs)
    {
        float timeStep = MathF.Min(deltaMs, 16.67f); // Clamping to 60 FPS max step

        // Movement
        if (_input.IsKeyDown(KeyCode.W)) Transform.Position += new Vector3(0, _speed * timeStep, 0);
        if (_input.IsKeyDown(KeyCode.S)) Transform.Position += new Vector3(0, -_speed * timeStep, 0);
        if (_input.IsKeyDown(KeyCode.D)) Transform.Position += new Vector3(_speed * timeStep, 0, 0);
        if (_input.IsKeyDown(KeyCode.A)) Transform.Position += new Vector3(-_speed * timeStep, 0, 0);        

        // Rotation
        if (_input.IsKeyDown(KeyCode.Left)) Transform.Rotation += new Vector3(0, 0, 0.1f * timeStep / 100);
        if (_input.IsKeyDown(KeyCode.Right)) Transform.Rotation -= new Vector3(0, 0, 0.1f * timeStep / 100);

        // Scale
        if (_input.IsKeyDown(KeyCode.Up)) Transform.Scale += new Vector3(0.001f * timeStep, 0.001f * timeStep, 0);
        if (_input.IsKeyDown(KeyCode.Down)) Transform.Scale -= new Vector3(0.001f * timeStep, 0.001f * timeStep, 0);
    }

    public override void Render(Renderer renderer, Camera camera)
    {
        _sprite!.Render(renderer, camera, Transform);
    }
}

public class Asteroid : Entity
{
    private IFireballContext _context;
    private IInput _input;
    private IAssetManager _assetManager;
    private Sprite _sprite;
    // private float _speed = .5f;

    public Asteroid(IFireballContext context, IInput input, IAssetManager assetManager)
    {
        _context = context;
        _input = input;        
        _assetManager = assetManager;
        CameraLayer = CameraLayer.Game;
    }

    public override async Task LoadAsync()
    {
        // Load assets
        var shader = _context.CreateShader(ShaderType.Sprite);
        shader.Compile();

        var material = new Material(shader, Color.White);
        var texture = await _assetManager.Load<Texture2D>("/asteroid/idle", "assets/sprites/asteroid_large.png");

        float spriteWidth = 128;
        float spriteHeight = 124;

        Transform.Position = new Vector3(400, 300, 0);

        _sprite = new Sprite(new Material(shader, Color.White), texture, spriteWidth, spriteHeight);
    }

    public override void Update(float deltaMs)
    {
        float timeStep = MathF.Min(deltaMs, 16.67f); // Clamping to 60 FPS max step

        // // Movement
        // if (_input.IsKeyDown(KeyCode.W)) Transform.Position += new Vector3(0, _speed * timeStep, 0);
        // if (_input.IsKeyDown(KeyCode.S)) Transform.Position += new Vector3(0, -_speed * timeStep, 0);
        // if (_input.IsKeyDown(KeyCode.D)) Transform.Position += new Vector3(_speed * timeStep, 0, 0);
        // if (_input.IsKeyDown(KeyCode.A)) Transform.Position += new Vector3(-_speed * timeStep, 0, 0);        

        // // Rotation
        // if (_input.IsKeyDown(KeyCode.Left)) Transform.Rotation += new Vector3(0, 0, 0.1f * timeStep / 100);
        Transform.Rotation += new Vector3(0, 0, 0.1f * timeStep / 100);
        // if (_input.IsKeyDown(KeyCode.Right)) Transform.Rotation -= new Vector3(0, 0, 0.1f * timeStep / 100);

        // // Scale
        // if (_input.IsKeyDown(KeyCode.Up)) Transform.Scale += new Vector3(0.001f * timeStep, 0.001f * timeStep, 0);
        // if (_input.IsKeyDown(KeyCode.Down)) Transform.Scale -= new Vector3(0.001f * timeStep, 0.001f * timeStep, 0);
    }

    public override void Render(Renderer renderer, Camera camera)
    {
        _sprite!.Render(renderer, camera, Transform);
    }
}