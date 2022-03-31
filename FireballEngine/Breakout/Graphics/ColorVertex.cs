using FireballEngine.Core.Graphics;
using FireballEngine.Core.Math;

namespace Breakout.Graphics
{
    public struct ColorVertex
    {
        public Vector4 Color { get; init; }
        public Vector3 Position { get; init; }

        public ColorVertex(Color color, Vertex position)
        {
            Color = color.ToVector4();
            Position = position.ToVector3();
        }
    }
}
