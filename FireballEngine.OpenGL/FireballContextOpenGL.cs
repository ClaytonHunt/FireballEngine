using System.Threading.Tasks;
using FireballEngine.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FireballEngine.OpenGL;

public class FireballContextOpenGL : IFireballContext
{
    public event UpdateHandler? OnUpdate;
    public event Action? OnRender;

    private GameWindow? _window;
    private Color? _lastClearColor;

    public async Task InitializeAsync(int width, int height, string title)
    {

        var gwSettings = new GameWindowSettings();
        var nativeWindowSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(width, height),
            Title = title
        };

        _window = new GameWindow(gwSettings, nativeWindowSettings);
        _window.UpdateFrame += OnUpdateFrame;
        _window.RenderFrame += OnRenderFrame;
        _window.Run(); // This will block the thread

        await Task.CompletedTask;
    }

    private void OnUpdateFrame(FrameEventArgs args)
    {
        float deltaTime = (float)(args.Time * 1000f);
        OnUpdate?.Invoke(deltaTime);
    }

    private void OnRenderFrame(FrameEventArgs args)
    {
        OnRender?.Invoke();
        _window?.SwapBuffers();
    }

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