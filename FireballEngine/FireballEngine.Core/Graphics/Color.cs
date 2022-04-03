using FireballEngine.Core.Math;

namespace FireballEngine.Core.Graphics
{
    public class Color
    {
        private Vector4 _position;

        private static Random _random = new Random();

        public float Red => _position.X;
        public float Green => _position.Y;
        public float Blue => _position.Z;
        public float Alpha => _position.W;

        public Color(int red, int green, int blue, float alpha)
        {
            _position = new Vector4(red / 255f, green / 255f, blue / 255f, alpha);
        }

        public Color(float red, float green, float blue, float alpha)
        {
            _position = new Vector4(red, green, blue, alpha);            
        }

        public Color(int hex, float alpha) : this((hex & 0xFF0000) >> 16, (hex & 0x00FF00) >> 8, (hex & 0x0000FF), alpha) { }

        public Vector4 ToVector4()
        {
            return _position;
        }

        public static Color Random()
        {
            return new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), 1.0f);
        }

        public static Color CornFlowerBlue => new Color(0.392f, 0.584f, 0.929f, 1.0f);
        public static Color Cyan => new Color(0x00FFFF, 1.0f);
        public static Color PureBlue => new Color(0x0000FF, 1.0f);
        public static Color PureGreen => new Color(0x00FF00, 1.0f);
        public static Color PureRed => new Color(0xFF0000, 1.0f);
        public static Color Orange => new Color(0xFFA500, 1.0f);
        public static Color Yellow => new Color(0xFFFF00, 1.0f);
        public static Color Violet => new Color(0xEE82EE, 1.0f);
        
    }
}
