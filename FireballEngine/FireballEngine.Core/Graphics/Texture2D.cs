using FireballEngine.Core.Math;

namespace FireballEngine.Core.Graphics
{
    public class Texture2D
    {
        public string Name { get; init; } = string.Empty;
        public string Source { get; init; } = string.Empty;
        public int Width { get; init; }
        public int Height { get; init; }

        private float _widthRatio;
        private float _heightRatio;

        public Texture2D(string name, string source, int width, int height)
        {
            Name = name;
            Source = source;
            Width = width;
            Height = height;

            _widthRatio = 1 / (float)Width;
            _heightRatio = 1 / (float)Height;
        }

        public Vector2 GetCoordinate(int pixelX, int pixelY)
        {
            return new Vector2(pixelX * _widthRatio, pixelY * _heightRatio);
        }
    }
}
