using FireballEngine.Core;
using FireballEngine.Core.Math;
using OpenTK.Graphics.OpenGL4;

namespace FireballEngine.OpenGL;

public class OpenGLRenderer : Renderer
{
    private int _vao;
    private int _vbo;    

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

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public override void DrawTriangle(Material material, float[] modelMatrix)
    {
        material.Use();        
        material.Shader.SetMatrix("model", modelMatrix);        
        float[] identityMatrix = new float[16];

        // Identity matrix (OpenGL column-major)
        identityMatrix[0] = 1.0f;
        identityMatrix[5] = 1.0f;
        identityMatrix[10] = 1.0f;
        identityMatrix[15] = 1.0f;
        
        material.Shader.SetMatrix("view", identityMatrix);

        float left = 0.0f, right = 800.0f;
        float bottom = 0.0f, top = 600.0f;
        float near = -1.0f, far = 1.0f;

        float[] projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);
        material.Shader.SetMatrix("projection", projectionMatrix);

        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.BindVertexArray(0);
    }
}