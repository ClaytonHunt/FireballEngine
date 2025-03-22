
namespace FireballEngine.Core.Math;

public struct Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3 Zero => new Vector3(0, 0, 0);
    public static Vector3 One => new Vector3(1, 1, 1);
    public static Vector3 Up => new Vector3(0, 1, 0);
    

    public Vector3 Normalize()
    {
        float length = (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
        return new Vector3(X / length, Y / length, Z / length);
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        return (a - b).Length();
    }

    public float Length()
    {
        return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public float LengthSquared()
    {
        return X * X + Y * Y + Z * Z;
    }

    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }

    public static bool operator ==(Vector3 a, Vector3 b)
    {
        return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    }

    public static bool operator !=(Vector3 a, Vector3 b)
    {
        return a.X != b.X || a.Y != b.Y || a.Z != b.Z;        
    }

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3 operator *(Vector3 a, float scalar)
    {
        return new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);
    }

    public static Vector3 operator /(Vector3 a, float scalar)
    {
        return new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);
    }

    public static Vector3 operator -(Vector3 a)
    {
        return new Vector3(-a.X, -a.Y, -a.Z);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Vector3 other)
        {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}