using FireballEngine.Core;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;
using OpenTK.Graphics.OpenGL4;

namespace FireballEngine.OpenGL.Rendering;

public class OpenGLRenderer : Renderer
{
    private int _vao;
    private int _vbo;
    private Color? _lastClearColor;

    private float[] _vertices = {
        0.0f,  66.7f,  0.0f,  // Top (450 - 383.3)
        -50.0f, -33.3f,  0.0f,  // Bottom Left (350 - 383.3)
        50.0f, -33.3f,  0.0f   // Bottom Right (450 - 383.3)
    };

    public OpenGLRenderer()
    {
        // Generate and bind VAO/VBO ONCE (not every frame)
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    /// <summary>
    /// Clears the screen with the specified color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public override void Clear(Color color)
    {
        if (!_lastClearColor.HasValue || !_lastClearColor.Value.Equals(color))
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
            _lastClearColor = color;
        }

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public override void DrawTriangle()
    {
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.BindVertexArray(0);
    }

    public override void DrawSprite(Texture2D texture, Material material, float x, float y, float width, float height, SpriteOrigin origin = SpriteOrigin.Center, float customOriginX = 0.5f, float customOriginY = 0.5f)
    {
        material.Use();
        texture.Bind();

        // Compute the origin offset
        (float originX, float originY) = GetOriginOffset(origin, customOriginX, customOriginY);

        var offsetX = width * originX;
        var offsetY = height * originY;

        // Adjust vertex positions based on the origin
        float[] vertices =
        {
            x - offsetX, y + height - offsetY, 0.0f, 0.0f, 1.0f, // Top Left
            x - offsetX, y - offsetY, 0.0f, 0.0f, 0.0f, // Bottom Left
            x + width - offsetX, y - offsetY, 0.0f, 1.0f, 0.0f, // Bottom Right
            x - offsetX, y + height - offsetY, 0.0f, 0.0f, 1.0f, // Top Left
            x + width - offsetX, y - offsetY, 0.0f, 1.0f, 0.0f, // Bottom Right
            x + width - offsetX, y + height - offsetY, 0.0f, 1.0f, 1.0f // Top Right
        };

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        int posAttrib = GL.GetAttribLocation(material.Shader.ProgramId, "aPos");
        GL.VertexAttribPointer(posAttrib, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posAttrib);

        int texAttrib = GL.GetAttribLocation(material.Shader.ProgramId, "aTexCoord");
        GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(texAttrib);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    private (float, float) GetOriginOffset(SpriteOrigin origin, float customX, float customY)
    {
        switch (origin)
        {
            case SpriteOrigin.TopLeft: return (0.0f, 0.0f);
            case SpriteOrigin.TopCenter: return (0.5f, 0.0f);
            case SpriteOrigin.TopRight: return (1.0f, 0.0f);
            case SpriteOrigin.MiddleLeft: return (0.0f, 0.5f);
            case SpriteOrigin.Center: return (0.5f, 0.5f);
            case SpriteOrigin.MiddleRight: return (1.0f, 0.5f);
            case SpriteOrigin.BottomLeft: return (0.0f, 1.0f);
            case SpriteOrigin.BottomCenter: return (0.5f, 1.0f);
            case SpriteOrigin.BottomRight: return (1.0f, 1.0f);
            case SpriteOrigin.Custom: return (customX, customY);
            default: return (0.5f, 0.5f); // Default to center
        }
    }
}