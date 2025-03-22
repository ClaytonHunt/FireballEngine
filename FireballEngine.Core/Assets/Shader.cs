namespace FireballEngine.Core.Assets;

/// <summary>
/// Base class for shader objects in Fireball Engine
/// </summary>
public abstract class Shader 
{
    public int ProgramId { get; protected set; } = -1;
    public string VertexSource { get; }
    public string FragmentSource { get; }

    protected Shader(string vertexSource, string fragmentSource)
    {
        VertexSource = vertexSource;
        FragmentSource = fragmentSource;
    }

    /// <summary>
    /// Compiles and initializes the shader.
    /// </summary>
    public abstract void Compile();

    /// <summary>
    /// Activates the shader for rendering.
    /// </summary>
    public abstract void Use();

    /// <summary>
    /// Sets a uniform color for the shader.
    /// </summary>
    public abstract void SetColor(float r, float g, float b, float a);


    /// <summary>
    /// Sets a uniform transformation matrix for the shader.
    /// </summary>
    public abstract void SetMatrix(string uniformName, float[] matrix);
}