namespace FireballEngine.Core.Graphics
{
    public class Color
    {
        private float _red;
        private float _green;
        private float _blue;
        private float _alpha;

        private static Random _random = new Random();

        public float Red => _red;
        public float Green => _green;
        public float Blue => _blue;
        public float Alpha => _alpha;

        public Color(int red, int green, int blue, float alpha)
        {
            _red = red / 255f;
            _green = green / 255f;
            _blue = blue / 255f;
            _alpha = alpha;
        }

        public Color(float red, float green, float blue, float alpha)
        {
            _red = red;
            _green = green;
            _blue = blue;
            _alpha = alpha;
        }

        public Color(int hex, float alpha) : this((hex & 0xFF0000) >> 16, (hex & 0x00FF00) >> 8, (hex & 0x0000FF), alpha) { }

        public static Color Random()
        {
            return new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), 1.0f);
        }

        public static Color CornFlowerBlue => new Color(0.392f, 0.584f, 0.929f, 1.0f);
    }
}
