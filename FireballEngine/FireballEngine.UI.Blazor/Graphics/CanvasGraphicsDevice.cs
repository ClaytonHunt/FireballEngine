using Microsoft.JSInterop;

namespace FireballEngine.Core.Graphics
{
    public class CanvasGraphicsDevice : IGraphicsDevice
    {
        private readonly IJSRuntime _jsRuntime;
        private TimeSpan totalGameTime = new TimeSpan();
        private IJSObjectReference? _module;

        protected Guid CanvasId { get; private set; }
        protected PresentationParameters PresentationParameters { get; private set; } = new PresentationParameters();

        public event Func<GameTime, Task> OnReady = gameTime => Task.CompletedTask;

        public CanvasGraphicsDevice(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void Configure(Guid id, int width, int height)
        {
            CanvasId = id;

            PresentationParameters = new PresentationParameters()
            {
                BackbufferWidth = width,
                BackbufferHeight = height,
            };
        }

        public async Task Initialize()
        {
            Console.WriteLine("Initializing Graphics");

            var tempModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/FireballEngine.UI.Blazor/fireball.js");
            _module = await tempModule.InvokeAsync<IJSObjectReference>("createEngine", CanvasId, PresentationParameters, DotNetObjectReference.Create(this));
        }

        public async Task Clear(Color color)
        {
            if (_module == null)
                return;

            await _module.InvokeVoidAsync("clear", color.Red, color.Green, color.Blue, color.Alpha);
        }

        public async Task LoadShaders(string vertexShaderSource, string fragmentShaderSource)
        {
            if (_module == null)
                return;

            await _module.InvokeVoidAsync("loadShaderProgram", vertexShaderSource, fragmentShaderSource);
        }

        [JSInvokable]
        public async Task Render(float timestamp)
        {
            var elapsed = new TimeSpan((long)(timestamp * 10000));
            totalGameTime = totalGameTime.Add(elapsed);

            await OnReady(new GameTime { ElapsedGameTime = elapsed, TotalGameTime = totalGameTime });
        }
    }
}
