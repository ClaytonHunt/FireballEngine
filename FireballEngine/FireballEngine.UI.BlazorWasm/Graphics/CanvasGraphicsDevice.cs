using Microsoft.JSInterop;

namespace FireballEngine.Core.Graphics
{
    public class CanvasGraphicsDevice : IGraphicsDevice
    {
        private readonly IJSRuntime _jsRuntime;
        private TimeSpan totalGameTime = new TimeSpan();
        private IJSObjectReference _module;

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

            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "_content/FireballEngine.UI.BlazorWasm/fireball.js");

            var result = _module is not null ? await _module.InvokeAsync<string>("showPrompt", "Hello World!!") : null;

            await _jsRuntime.InvokeVoidAsync("eval", $@"
                    window.Fireball = window.Fireball || {{}}; 
                    window.Fireball['{CanvasId}'] = window.Fireball['{CanvasId}'] || {{}};
                    window.Fireball['{CanvasId}'].canvas = document.getElementById('{CanvasId}');
                    window.Fireball['{CanvasId}'].canvas.width = {PresentationParameters.BackbufferWidth};
                    window.Fireball['{CanvasId}'].canvas.height = {PresentationParameters.BackbufferHeight};
                    window.Fireball['{CanvasId}'].ctx = window.Fireball['{CanvasId}'].canvas.getContext('webgl');
                    window.Fireball['{CanvasId}'].setDotnetGraphics = (dotnethelper) => {{
                        window.Fireball['{CanvasId}'].dotnetGraphics = dotnethelper;
                    }};
                    window.Fireball['{CanvasId}'].render = (timestamp) => {{
                        const currentTimeStamp = timestamp - previousTimestamp;
                        previousTimestamp = timestamp;
                        
                        dotnetGraphics.invokeMethodAsync('Render', currentTimeStamp)
                            .then(_ => window.requestAnimationFrame(window.Fireball['{CanvasId}'].render));
                    }};");

            //await _jsRuntime.InvokeVoidAsync($"window.Fireball['{CanvasId}'].setDotnetGraphics", DotNetObjectReference.Create(this));

            //await _jsRuntime.InvokeVoidAsync($"eval", $"window.requestAnimationFrame(window.Fireball['{CanvasId}'].render);");
        }

        public async Task Clear(Color color)
        {            
            await _jsRuntime.InvokeVoidAsync("eval", $@"
                    window.Fireball['{CanvasId}'].ctx.clearColor({color.Red}, {color.Green}, {color.Blue}, {color.Alpha});
                    window.Fireball['{CanvasId}'].ctx.clear(window.Fireball['{CanvasId}'].ctx.COLOR_BUFFER_BIT);");
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
