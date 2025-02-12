using System.Diagnostics.CodeAnalysis;

namespace FireballEngine.Core;

public struct Color 
{
    public float R, G, B, A;

    public Color(float r, float g, float b, float a = 1.0f)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public static Color White => new Color(1.0f, 1.0f, 1.0f);
    public static Color Black => new Color(0.0f, 0.0f, 0.0f);
    public static Color CornflowerBlue => new Color(0.392f, 0.584f, 0.929f);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Color color &&
               R == color.R &&
               G == color.G &&
               B == color.B &&
               A == color.A;
    }

    public override int GetHashCode() => (R, G, B, A).GetHashCode();
}