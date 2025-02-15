namespace FireballEngine.Core;

/// <summary>
/// A material represents a shader and it's properties.
/// </summary>
public class Material
{
    public Shader Shader { get; }
    public Color Color { get; set; }

    public Material(Shader shader, Color color)
    {
        Shader = shader;
        Color = color;
    }

    public void Use() 
    {
        Shader.Use();
        Shader.SetColor(Color.R, Color.G, Color.B, Color.A);
    }
}