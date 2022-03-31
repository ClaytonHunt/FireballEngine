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
        public Task DrawPrimitives(string alias, int offset, int size);
    }
}