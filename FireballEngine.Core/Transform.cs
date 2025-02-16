using FireballEngine.Core.Math;

namespace FireballEngine.Core;

public class Transform
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;

    public Matrix4 GetTransformationMatrix()
    {
        return Matrix4.CreateScale(Scale) *
               Matrix4.CreateRotation(Rotation) *
               Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z);
    }
}

