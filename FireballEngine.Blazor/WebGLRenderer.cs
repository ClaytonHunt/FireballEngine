using FireballEngine.Core;
using FireballEngine.Core.Graphics;
using FireballEngine.Core.Math;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor;

public class WebGLRenderer : Renderer
{
    private readonly IJSObjectReference _jsModule;

    public WebGLRenderer(IJSObjectReference jsModule)
    {
        _jsModule = jsModule;
    }

    public override async void DrawTriangle()
    {   
        await _jsModule.InvokeVoidAsync("drawTriangle");
    }
}