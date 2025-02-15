using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FireballEngine.OpenGL.Assets;

public class OpenGLTexture2D : Texture2D, IOpenGLAsset<Texture2D>
{
    private int _textureId;
    public int TextureId
    {
        get => _textureId;
        private set => _textureId = value;
    }

    public OpenGLTexture2D(string name, string path) : base(name, path)
    {
        LoadTextureFromFile(path);
    }

    public static Task<Texture2D> Load(string name, string path)
    {
        int textureId = LoadTextureFromFile(path);
        return Task.FromResult<Texture2D>(new OpenGLTexture2D(name, path) { TextureId = textureId });
    }

    public override void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, _textureId);
    }

    public override void Dispose()
    {
        GL.DeleteTexture(_textureId);
    }

    public override Task<bool> IsLoaded()
    {
        return Task.FromResult(_textureId != 0);
    }

    private static int LoadTextureFromFile(string path)
    {        
        FireballMessage.Info($"Loading texture from file: {path}");

        if (!File.Exists(path))
        {
            FireballMessage.Error($"Texture file not found: {path}");
            throw new FileNotFoundException($"Texture file not found: {path}");
        }                

        int textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);        

        using (var image = Image.Load<Rgba32>(path))
        {
            image.Mutate(x => x.Flip(FlipMode.Vertical));            

            var pixels = new byte[image.Width * image.Height * 4];            

            image.CopyPixelDataTo(pixels);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte,
                          pixels);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        FireballMessage.Info($"Loaded texture from file: {path}");

        return textureId;
    }
}