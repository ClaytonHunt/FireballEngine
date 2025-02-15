using OpenTK.Graphics.OpenGL4;
using FireballEngine.Core.Graphics;

namespace FireballEngine.OpenGL
{
    public class OpenGLTexture2D : Texture2D
    {
        public OpenGLTexture2D(int width, int height, int textureId) : base(width, height, textureId) {}

        public override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
        }
    }
}