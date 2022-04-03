namespace FireballEngine.Core.Graphics
{
    public interface IGraphicsDevice
    {
        public event Func<GameTime, Task> OnFrame;        
        public void Configure(Guid id, int width, int height);
        public Task Initialize();
        public Task Clear(Color color);
        public Task LoadShaders(string vertexShader, string fragmentShader);
        public Task CreatePrimitives<T>(string alias, IEnumerable<T> data, params int[] sizes);
        public Task CreateShape(string alias, IEnumerable<int> data);
        public Task DrawShape(string alias, int offset, int size);
        public Task DrawShapeOutline(string alias, int offset, int size);
        public Task DrawTriangles(string alias, int offset, int size);
        public Task DrawLines(string alias, int offset, int size);        
    }
}