using Microsoft.JSInterop;

namespace FireballEngine.Core.Graphics
{
    public class CanvasGraphicsDevice : IGraphicsDevice
    {
        private readonly IJSRuntime _jsRuntime;

        protected Guid CanvasId { get; private set; }
        protected PresentationParameters PresentationParameters { get; private set; } = new PresentationParameters();

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

            await _jsRuntime.InvokeVoidAsync("eval", $"window.Fireball = window.Fireball || {{}}; window.Fireball['{CanvasId}'] = window.Fireball['{CanvasId}'] || {{}};");

            await _jsRuntime.InvokeVoidAsync("eval", $@"
            window.Fireball['{CanvasId}'].canvas = document.getElementById('{CanvasId}');
            window.Fireball['{CanvasId}'].canvas = document.getElementById('{CanvasId}');");

            await _jsRuntime.InvokeVoidAsync("eval", $"window.Fireball['{CanvasId}'].ctx = window.Fireball['{CanvasId}'].canvas.getContext('webgl');");

            // Get Canvas by Id
            // var canvas = document.getElementById(renderTarget.id);

            // Set Width and Height
            //canvas.height = renderTarget.height;
            //canvas.width = renderTarget.width;

            // Create the webgl context
            //var ctx = canvas.getContext("webgl");
        }

        public async Task Clear(float red, float green, float blue, float alpha)
        {            
            await _jsRuntime.InvokeVoidAsync("eval", $"window.Fireball['{CanvasId}'].ctx.clearColor({red}, {green}, {blue}, {alpha})");
            await _jsRuntime.InvokeVoidAsync("eval", $"window.Fireball['{CanvasId}'].ctx.clear(window.Fireball['{CanvasId}'].ctx.COLOR_BUFFER_BIT)");
        }
    }
}
