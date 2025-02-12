using FireballEngine.Core;
using OpenTK.Graphics.OpenGL4;

namespace FireballEngine.OpenGL;

public class OpenGLShader : Shader
{    
    private bool _isCompiled => ProgramId != -1;

    public OpenGLShader(string vertexSource, string fragmentSource) : base(vertexSource, fragmentSource) { }

    public override void Compile()
    {
        if (_isCompiled) return;

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, VertexSource);
        GL.CompileShader(vertexShader);
        CheckCompileErrors(vertexShader, "VERTEX");

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
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