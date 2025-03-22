using FireballEngine.Core.Assets;
using OpenTK.Graphics.OpenGL4;
using GLShaderType = OpenTK.Graphics.OpenGL4.ShaderType;

namespace FireballEngine.OpenGL;

public class OpenGLShader : Shader
{
    private bool _isCompiled => ProgramId != -1;

    public OpenGLShader(string vertexSource, string fragmentSource) : base(vertexSource, fragmentSource) { }

    public override void Compile()
    {
        if (_isCompiled) return;

        int vertexShader = GL.CreateShader(GLShaderType.VertexShader);
        GL.ShaderSource(vertexShader, VertexSource);
        GL.CompileShader(vertexShader);
        CheckCompileErrors(vertexShader, "VERTEX");

        int fragmentShader = GL.CreateShader(GLShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, FragmentSource);
        GL.CompileShader(fragmentShader);
        CheckCompileErrors(fragmentShader, "FRAGMENT");

        ProgramId = GL.CreateProgram();
        GL.AttachShader(ProgramId, vertexShader);
        GL.AttachShader(ProgramId, fragmentShader);
        GL.LinkProgram(ProgramId);
        CheckLinkErrors(ProgramId);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public override void Use()
    {
        GL.UseProgram(ProgramId);
    }

    public override void SetColor(float r, float g, float b, float a)
    {
        int colorLocation = GL.GetUniformLocation(ProgramId, "uColor");
        GL.Uniform4(colorLocation, r, g, b, a);
    }

    public override void SetMatrix(string uniformName, float[] matrix)
    {
        if (!_isCompiled) return;
        int location = GL.GetUniformLocation(ProgramId, uniformName);
        if (location == -1)
        {
            Console.WriteLine($"[Fireball OpenGL] Uniform '{uniformName}' not found.");
            return;
        }
        GL.UniformMatrix4(location, 1, false, matrix);
    }

    private void CheckCompileErrors(int shader, string type)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);

        if (success == 0)
        {
            GL.GetShaderInfoLog(shader, out string infoLog);

            throw new Exception($"Error compiling {type} shader: {infoLog}");
        }
    }

    private void CheckLinkErrors(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);

        if (success == 0)
        {
            GL.GetProgramInfoLog(program, out string infoLog);
            throw new Exception($"Error linking program: {infoLog}");
        }
    }
}

public class OpenGLBasicColorShader : OpenGLShader
{
    public OpenGLBasicColorShader() : base(vertexSource, fragmentSource) { }

    private const string vertexSource = @"#version 330 core
        layout (location = 0) in vec3 aPos; // Vertex position

        // Matrices
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;

        void main()
        {
            // Apply transformations in the correct order: Model -> View -> Projection
            vec4 worldPos = model * vec4(aPos, 1.0);
            vec4 clipSpacePos = projection * view * worldPos;
            gl_Position = clipSpacePos;
        }";

    private const string fragmentSource = @"#version 330 core
        out vec4 FragColor;
        
        uniform vec4 uColor;        
        
        void main() {
            FragColor = uColor;
        }";
}

public class OpenGLSpriteShader : OpenGLShader
{
    public OpenGLSpriteShader() : base(vertexSource, fragmentSource) { }

    private const string vertexSource = @"#version 330 core
        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec2 aTexCoord;
        
        // Matrices
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;
        
        out vec2 TexCoord;
        
        void main() {
            vec4 worldPos = model * vec4(aPos, 1.0);
            vec4 clipSpacePos = projection * view * worldPos;
            gl_Position = clipSpacePos;
            TexCoord = aTexCoord;
        }";

    private const string fragmentSource = @"#version 330 core
        in vec2 TexCoord;

        out vec4 FragColor;
        
        uniform sampler2D uTexture;

        void main() {
            FragColor = texture(uTexture, TexCoord);
        }";
}