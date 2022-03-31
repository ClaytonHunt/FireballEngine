using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireballEngine.Core.Math
{
    public struct Vector4 : IEquatable<Vector4>
    {        
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4(Vector2 value, float z, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 value, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        public Vector4(float value)
        {
            X = Y = Z = W = value;
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public bool Equals(Vector4 other)
        {
            throw new NotImplementedException();
        }
    }

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

        public bool Equals(Vector2 other)
        {
            throw new NotImplementedException();
        }
    }
}
