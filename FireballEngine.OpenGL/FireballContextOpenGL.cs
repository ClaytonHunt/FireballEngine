using FireballEngine.Core;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;
using FireballEngine.OpenGL.Assets;
using FireballEngine.OpenGL.Utilities;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using ShaderType = FireballEngine.Core.ShaderType;

namespace FireballEngine.OpenGL;

public class FireballContextOpenGL : IFireballContext
{
    private GameWindow? _window;    
    public IGame Game { get; }
    public IInput Input { get; private set; } = null!;
    public Renderer Renderer { get; private set; } = null!;
    public IAssetManager AssetManager { get; private set; } = null!;

    public FireballContextOpenGL(IGame game)
    {
        Fire.SetFormatter(new TerminalLogFormatter());

        Game = game;          
    }

    public async Task InitializeAsync(int width, int height, string title)
    {
        var gwSettings = new GameWindowSettings();
        var nativeWindowSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(width, height),
            Title = title
        };

        _window = new GameWindow(gwSettings, nativeWindowSettings);
        // Instead of hooking user events, call TGame directly:
        _window.Load += HandleOnLoad;
        _window.KeyDown += OnKeyDown;
        _window.KeyUp += OnKeyUp;
        _window.UpdateFrame += OnUpdateFrame;
        _window.RenderFrame += OnRenderFrame;

        Input = new OpenGLInput();
        Renderer = new OpenGLRenderer();
        AssetManager = new OpenGLAssetManager();      

        // Starting this will block the current thread until the window closes.
        _window.Run();

        await Task.CompletedTask;
    }

    /// <summary>
    /// Called when the OpenGL windows is ready. Triggers the OnLoad event for the user to set up shaders and renderers.
    /// </summary>
    private void HandleOnLoad()
    {
        Game.OnLoad(this);
    }

    /// <summary>
    /// Called when a key is pressed.
    /// </summary>
    /// <param name="e"></param>
    private void OnKeyDown(KeyboardKeyEventArgs e)
    {
        ((OpenGLInput)Input).OnKeyDown(e);
    }

    /// <summary>
    /// Called when a key is released.
    /// </summary>
    private void OnKeyUp(KeyboardKeyEventArgs e)
    {
        ((OpenGLInput)Input).OnKeyUp(e);
    }

    /// <summary>
    /// This method is called every frame. It is responsible for updating the game state.
    /// </summary>
    /// <param name="args"></param>

    private void OnUpdateFrame(FrameEventArgs args)
    {
        float deltaMs = (float)(args.Time * 1000.0f);
        Fire.Info($"Delta: {deltaMs}");
        Game.Update(deltaMs);
    }

    /// <summary>
    /// This method is called every frame. It is responsible for rendering the game.
    /// </summary>
    private void OnRenderFrame(FrameEventArgs args)
    {
        Game.Render();

        _window?.SwapBuffers();
    }

    /// <summary>
    /// Creates a new OpenGL shader.
    /// </summary>
    /// <param name="vertexSource"></param>
    /// <param name="fragmentSource"></param>
    /// <returns></returns>
    public Shader CreateShader(ShaderType type)
    {
        switch(type)
        {
            case ShaderType.BasicColor:
                return new OpenGLBasicColorShader();
            case ShaderType.Sprite:
                return new OpenGLSpriteShader();
            default:
                throw new ArgumentException("Invalid shader type.");
        }
    }
}
