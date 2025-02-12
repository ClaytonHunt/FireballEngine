using FireballEngine.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FireballEngine.OpenGL
{
    public class FireballContextOpenGL : IFireballContext        
    {
        private GameWindow? _window;
        private Color? _lastClearColor;

        public IGame Game { get; }

        public FireballContextOpenGL(IGame game)
        {
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
            _window.UpdateFrame += OnUpdateFrame;
            _window.RenderFrame += OnRenderFrame;

            // Starting this will block the current thread until the window closes.
            _window.Run();

            await Task.CompletedTask;
        }

        private void OnUpdateFrame(FrameEventArgs args)
        {
            float deltaMs = (float)(args.Time * 1000.0f);
            Game.Update(deltaMs);
        }

        private void OnRenderFrame(FrameEventArgs args)
        {
            // The game is responsible for telling the context what to do 
            // (e.g., clearing the screen). We call Game.Render(this).
            Game.Render(this);

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
}
