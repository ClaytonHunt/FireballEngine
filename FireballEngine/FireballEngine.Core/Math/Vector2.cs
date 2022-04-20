using Mathematics = System.Math;

namespace FireballEngine.Core.Math
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;

        public static Vector2 One { get; } = new Vector2(1, 1);
        public static Vector2 UnitX { get; } = new Vector2(1, 0);
        public static Vector2 UnitY { get; } = new Vector2(0, 1);
        public static Vector2 Zero { get; } = new Vector2(0, 0);

        public Vector2(float value)
        {
            X = value;
            Y = value;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            return vector / vector.Magnitude();
        }

        public float Magnitude()
        {
            return (float)Mathematics.Sqrt(Mathematics.Pow(X, 2) + Mathematics.Pow(Y, 2));
        }        

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public static Vector2 operator /(Vector2 vector, double value)
        {
            return new Vector2(vector.X / (float)value, vector.Y / (float)value);
        }

        public static float crossProduct(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }
    }
}
