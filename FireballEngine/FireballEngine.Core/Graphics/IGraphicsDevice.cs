namespace FireballEngine.Core.Graphics
{
    public interface IGraphicsDevice
    {
        public void Configure(Guid id, int width, int height);
        public Task Initialize();
        public Task Clear(float red, float green, float blue, float alpha);
    }    
}