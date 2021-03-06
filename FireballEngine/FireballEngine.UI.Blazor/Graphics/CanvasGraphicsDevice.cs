using FireballEngine.Core.Math;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace FireballEngine.Core.Graphics
{
    public class CanvasGraphicsDevice : IGraphicsDevice
    {
        public int Width => PresentationParameters.BackbufferWidth;
        public int Height => PresentationParameters.BackbufferHeight;

        private readonly IJSRuntime _jsRuntime;
        private TimeSpan totalGameTime = new TimeSpan();        

        protected Guid CanvasId { get; private set; }
        protected PresentationParameters PresentationParameters { get; private set; } = new PresentationParameters();

        public IJSObjectReference? JsModule;
        public event Func<GameTime, Task> OnFrame = gameTime => Task.CompletedTask;

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
            JsModule = await tempModule.InvokeAsync<IJSObjectReference>("createEngine", CanvasId, PresentationParameters, DotNetObjectReference.Create(this));
        }

        public async Task Clear(Color color)
        {
            if (JsModule == null)
                return;

            await JsModule.InvokeVoidAsync("clear", color.Red, color.Green, color.Blue, color.Alpha);
        }

        public async Task LoadShaders(string alias, string vertexShaderSource, string fragmentShaderSource)
        {
            if (JsModule == null) 
                return;

            await JsModule.InvokeVoidAsync("loadShaderProgram", alias, vertexShaderSource, fragmentShaderSource);
        }

        public Task SetUniformMatrix4(string shaderAlias, string uniformAlias, Matrix4 matrix)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, string, Matrix4>, object>("setUniformMatrix4", new(shaderAlias, uniformAlias, matrix)));
        }

        [JSInvokable]
        public async Task Render(float timestamp)
        {
            var elapsed = new TimeSpan((long)(timestamp * 10000));
            totalGameTime = totalGameTime.Add(elapsed);

            await OnFrame(new GameTime { ElapsedGameTime = elapsed, TotalGameTime = totalGameTime });
        }        

        public async Task CreatePrimitives<T>(string alias, IEnumerable<T> data, params int[] sizes)
        {
            await CreateVertexArray(alias);
            await CreateArrayBuffer(alias, data, sizes.Aggregate((a, b) => a + b));

            var dataSize = Marshal.SizeOf(typeof(T));

            for (int offset = 0, sizePosition = 0, location = 0; 
                 sizePosition < sizes.Length;
                 offset += (sizes[sizePosition] * sizeof(float)), sizePosition++, location++)
            {
                await CreateVertexAttribute(location, sizes[sizePosition], dataSize, offset);
            }
        }

        public async Task CreateShape<T>(string alias, IEnumerable<T> vertexData, IEnumerable<int> shapeData, params int[] sizes)
        {
            await CreatePrimitives(alias, vertexData, sizes);
            await CreateElementArrayBuffer(alias, shapeData);            
        }

        public Task CreateTexture(Texture2D texture)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, string>, object>("createTexture", new(texture.Name, texture.Source)));
        }

        public Task DrawTriangles(string alias, int offset, int size)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, int, int>, object>("drawTriangles", new (alias, offset, size)));
        }

        public Task DrawLines(string alias, int offset, int size)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, int, int>, object>("drawLines", new(alias, offset, size)));
        }

        public Task DrawShape(string alias, int offset, int size)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, int, int>, object>("drawElement", new(alias, offset, size)));
        }

        public Task DrawShapeOutline(string alias, int offset, int size)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, int, int>, object>("drawShapeOutline", new(alias, offset, size)));
        }

        private Task CreateVertexArray(string alias)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string>, object>("createVertexArray", new (alias)));
        }

        private Task CreateArrayBuffer<T>(string alias, IEnumerable<T> data, int size)
        {
            if (JsModule == null)
                return Task.CompletedTask;            

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, int, IEnumerable<T>>, object>("createArrayBuffer", new (alias, size, data)));
        }

        private Task CreateElementArrayBuffer(string alias, IEnumerable<int> data)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<string, IEnumerable<int>>, object>("createElementArrayBuffer", new(alias, data)));
        }

        private Task CreateVertexAttribute(int location, int size, int structureSize, int offset)
        {
            if (JsModule == null)
                return Task.CompletedTask;

            return Task.FromResult(((IJSUnmarshalledObjectReference)JsModule).InvokeUnmarshalled<ValueTuple<int, int, int, int>, object>("createVertexAttribute", new (location, size, structureSize, offset)));
        }        
    }
}
