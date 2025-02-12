using FireballEngine.Core;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor;

public class WebGLRenderer : Renderer 
{
    private readonly IJSObjectReference _jsModule;
    
    public WebGLRenderer(IJSObjectReference jsModule) 
    {
        _jsModule = jsModule;        
    }

    public override async void DrawTriangle(Material material, float[] modelMatrix) 
    {
        material.Use();
        material.Shader.SetMatrix("model", modelMatrix);
        await _jsModule.InvokeVoidAsync("drawTriangle");
    }
}