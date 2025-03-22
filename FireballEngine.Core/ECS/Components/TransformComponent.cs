using FireballEngine.Core.Math;

namespace FireballEngine.Core.ECS.Components
{
    /// <summary>
    /// Component that handles position, rotation, and scale of an entity.
    /// </summary>
    public class TransformComponent
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        /// Gets the transformation matrix based on position, rotation, and scale.
        /// </summary>
        public Matrix4 GetTransformationMatrix()
        {
            return Matrix4.CreateScale(Scale) *
                   Matrix4.CreateRotation(Rotation) *
                   Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z);
        }
    }
}
