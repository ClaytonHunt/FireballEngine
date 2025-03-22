namespace FireballEngine.Core.Utils
{
    /// <summary>
    /// Represents an RGBA color.
    /// </summary>
    public struct Color
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public static Color White => new Color(1, 1, 1, 1);
        public static Color Black => new Color(0, 0, 0, 1);
        public static Color Red => new Color(1, 0, 0, 1);
        public static Color Green => new Color(0, 1, 0, 1);
        public static Color Blue => new Color(0, 0, 1, 1);
        public static Color Yellow => new Color(1, 1, 0, 1);
        public static Color Cyan => new Color(0, 1, 1, 1);
        public static Color Magenta => new Color(1, 0, 1, 1);
        public static Color Transparent => new Color(0, 0, 0, 0);
        public static Color CornflowerBlue => new Color(0.392f, 0.584f, 0.929f);

        public Color(float r, float g, float b, float a = 1.0f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r / 255.0f;
            G = g / 255.0f;
            B = b / 255.0f;
            A = a / 255.0f;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Color other)
            {
                return R == other.R && G == other.G && B == other.B && A == other.A;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B, A);
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }

        public static Color Lerp(Color a, Color b, float t)
        {
            t = SMath.Clamp(t, 0.0f, 1.0f);
            return new Color(
                a.R + (b.R - a.R) * t,
                a.G + (b.G - a.G) * t,
                a.B + (b.B - a.B) * t,
                a.A + (b.A - a.A) * t
            );
        }

        public byte[] ToBytes()
        {
            return new byte[]
            {
                (byte)(R * 255),
                (byte)(G * 255),
                (byte)(B * 255),
                (byte)(A * 255)
            };
        }

        public override string ToString()
        {
            return $"RGBA({R:F2}, {G:F2}, {B:F2}, {A:F2})";
        }
    }
}
