namespace FireballEngine.Core.Graphics
{
    public interface IGraphicsDevice
    {
        public event Func<GameTime, Task> OnReady;        
        public void Configure(Guid id, int width, int height);
        public Task Initialize();
        public Task Clear(Color color);
        public Task LoadShaders(string vertexShader, string fragmentShader);

    }
}