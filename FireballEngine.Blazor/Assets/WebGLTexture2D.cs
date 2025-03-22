using Microsoft.JSInterop;

namespace FireballEngine.Blazor.Assets;

public class WebGLTexture2D : Core.Assets.Texture2D, IWebGLAsset<Core.Assets.Texture2D>
{
    private IJSObjectReference _jsModule;

    public WebGLTexture2D(string name, string path) : base(name, path)
    {
        throw new NotSupportedException("Use Load method to create WebGLTexture2D");
    }

    private WebGLTexture2D(IJSObjectReference jsModule, string name, string path) : base(name, path)
    {
        _jsModule = jsModule;
    }

    public static async Task<Core.Assets.Texture2D> Load(IJSObjectReference jsModule, string name, string path)
    {
        var texture = new WebGLTexture2D(jsModule, name, path);
        await jsModule.InvokeVoidAsync("preloadTexture", name, path);

        return texture;
    }

    public override async Task<bool> IsLoaded()
    {
        return await _jsModule.InvokeAsync<bool>("isTextureLoaded", Name);
    }

    public override void Bind()
    {
        _jsModule.InvokeVoidAsync("bindTexture", Name);
    }

    public override void Dispose()
    {
        _jsModule.InvokeVoidAsync("deleteTexture", Name);
    }
}
