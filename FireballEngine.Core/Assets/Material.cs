using FireballEngine.Core.Utils;

namespace FireballEngine.Core.Assets;

/// <summary>
/// Represents a material that defines how surfaces appear when rendered.
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
