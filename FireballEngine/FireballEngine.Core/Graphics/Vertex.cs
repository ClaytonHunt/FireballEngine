using FireballEngine.Core.Math;

namespace FireballEngine.Core.Graphics
{
    public class Vertex
    {
        private Vector3 _position;

        public float X => _position.X;
        public float Y => _position.Y;
        public float Z => _position.Z;

        public Vertex(float x, float y, float z)
        {
            _position = new Vector3(x, y, z);
        }

        public Vector3 ToVector3() 
        {
            return _position;
        }
    }
}
