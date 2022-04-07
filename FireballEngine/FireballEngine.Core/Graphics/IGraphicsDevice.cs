using FireballEngine.Core.Math;

namespace FireballEngine.Core.Graphics
{
    public interface IGraphicsDevice
    {
        public int Width { get; }
        public int Height { get; }
        public event Func<GameTime, Task> OnFrame;        
        public void Configure(Guid id, int width, int height);
        public Task Initialize();
        public Task Clear(Color color);
        public Task LoadShaders(string alias, string vertexShader, string fragmentShader);
        public Task SetUniformMatrix4(string shaderAlias, string uniformAlias, Matrix4 matrix);
        public Task CreatePrimitives<T>(string alias, IEnumerable<T> data, params int[] sizes);
        public Task CreateShape<T>(string alias, IEnumerable<T> vertexData, IEnumerable<int> shapeData, params int[] sizes);
        public Task DrawShape(string alias, int offset, int size);
        public Task DrawShapeOutline(string alias, int offset, int size);
        public Task DrawTriangles(string alias, int offset, int size);
        public Task DrawLines(string alias, int offset, int size);        
    }
}