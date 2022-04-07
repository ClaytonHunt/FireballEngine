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

        public bool Equals(Vector2 other)
        {
            throw new NotImplementedException();
        }
    }
}
