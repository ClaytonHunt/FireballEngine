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

    public override async void DrawTriangle(Material material, float[] modelMatrix)
    {
        material.Use();
        material.Shader.SetMatrix("model", modelMatrix);
        float[] identityMatrix = new float[16];

        // Identity matrix (OpenGL column-major)
        identityMatrix[0] = 1.0f;
        identityMatrix[5] = 1.0f;
        identityMatrix[10] = 1.0f;
        identityMatrix[15] = 1.0f;
        
        material.Shader.SetMatrix("view", identityMatrix);

        float left = 0.0f, right = 800.0f;
        float bottom = 0.0f, top = 600.0f;
        float near = -1.0f, far = 1.0f;

        float[] projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);
        material.Shader.SetMatrix("projection", projectionMatrix);
        
        await _jsModule.InvokeVoidAsync("drawTriangle");
    }
}