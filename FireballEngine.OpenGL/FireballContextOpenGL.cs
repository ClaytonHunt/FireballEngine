using FireballEngine.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FireballEngine.OpenGL;

public class FireballContextOpenGL : IFireballContext
{
    private GameWindow? _window;
    private Color? _lastClearColor;
    public IGame Game { get; }
    public IInput Input { get;}

    public FireballContextOpenGL(IGame game)
    {
        Game = game;
        Input = new OpenGLInput();
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
        Game.Update(deltaMs);
    }

    /// <summary>
    /// This method is called every frame. It is responsible for rendering the game.
    /// </summary>
    private void OnRenderFrame(FrameEventArgs args)
    {
        // The game is responsible for telling the context what to do 
        // (e.g., clearing the screen). We call Game.Render(this).
        Game.Render(this);

        _window?.SwapBuffers();
    }

    /// <summary>
    /// Creates a new OpenGL shader.
    /// </summary>
    /// <param name="vertexSource"></param>
    /// <param name="fragmentSource"></param>
    /// <returns></returns>
    public Shader CreateShader(string vertexSource, string fragmentSource)
    {
        return new OpenGLShader(vertexSource, fragmentSource);
    }


    /// <summary>
    /// Creates a new OpenGL renderer.
    /// </summary>
    public Renderer CreateRenderer()
    {
        return new OpenGLRenderer();
    }

    /// <summary>
    /// Clears the screen with the specified color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public async Task Clear(Color color)
    {
        if (!_lastClearColor.HasValue || !_lastClearColor.Value.Equals(color))
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
            _lastClearColor = color;
        }

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        await Task.CompletedTask;
    }
}
