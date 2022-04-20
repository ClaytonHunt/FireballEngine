using Mathematics = System.Math;

namespace FireballEngine.Core.Math
{
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;
        public float Y;
        public float Z;

        public static Vector3 Up = new(0, 1, 0);

        public Vector3(Vector2 value, float z)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
        }

        public Vector3(float value)
        {
            X = Y = Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            return vector / vector.Magnitude();
        }

        public float Magnitude()
        {
            return (float)Mathematics.Sqrt(Mathematics.Pow(X, 2) + Mathematics.Pow(Y, 2) + Mathematics.Pow(Z, 2));
        }

        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
            );
        }

        public bool Equals(Vector3 other)
        {
            throw new NotImplementedException();
        }

        public static Vector3 operator /(Vector3 vector, double value)
        {
            return new Vector3(vector.X / (float)value, vector.Y / (float)value, vector.Z / (float)value);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
    }
}
