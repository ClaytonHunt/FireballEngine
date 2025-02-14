using FireballEngine.Core;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor;

public class WebGLShader : Shader
{
    private readonly IJSObjectReference _jsModule;    
    private bool _isCompiled = false;

    public WebGLShader(IJSObjectReference jsModule, string vertexSource, string fragmentSource)
        : base(vertexSource, fragmentSource)
    {
        _jsModule = jsModule;
        ProgramId = ShaderCounter.Count;
    }

    public override async void Compile()
    {
        if (_isCompiled) return;

        await _jsModule.InvokeVoidAsync("createShader", ProgramId, VertexSource, FragmentSource);
        _isCompiled = true;
    }

    public override async void Use()
    {
        await _jsModule.InvokeVoidAsync("useShader", ProgramId);
    }

    public override async void SetColor(float r, float g, float b, float a)
    {
        await _jsModule.InvokeVoidAsync("setColor", ProgramId, r, g, b, a);
    }

    public override async void SetMatrix(string uniformName, float[] matrix)
    {
        await _jsModule.InvokeVoidAsync("setMatrix", ProgramId, uniformName, matrix);
    }
}