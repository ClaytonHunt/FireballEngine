using FireballEngine.Core;
using OpenTK.Graphics.OpenGL4;

namespace FireballEngine.OpenGL;

public class OpenGLRenderer : Renderer
{
    private int _vao;

    public OpenGLRenderer()
    {
        float[] verticies = {
            0.0f, 0.5f, 0.0f, // Top
            -0.5f, -0.5f, 0.0f, // Bottom left
            0.5f, -0.5f, 0.0f // Bottom right
        };

        _vao = GL.GenVertexArray();
        int vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, verticies.Length * sizeof(float), verticies, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public override void DrawTriangle(Material material, float[] modelMatrix)
    {
        material.Use();
        material.Shader.SetMatrix("model", modelMatrix);

        float[] vertices = [
            0.0f,  0.5f, 0.0f,  // Top
           -0.5f, -0.5f, 0.0f,  // Bottom Left
            0.5f, -0.5f, 0.0f   // Bottom Right
        ];

        int vao = GL.GenVertexArray();
        int vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        int positionAttribLocation = GL.GetAttribLocation(material.Shader.ProgramId, "aPos");
        if (positionAttribLocation == -1)
        {
            Console.WriteLine("[Fireball OpenGL] Attribute 'aPos' not found.");
            return;
        }

        GL.EnableVertexAttribArray(positionAttribLocation);
        GL.VertexAttribPointer(positionAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}