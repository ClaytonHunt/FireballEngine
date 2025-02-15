using FireballEngine.Core;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor;

public class WebGLShader : Shader
{
    private readonly IJSObjectReference _jsModule;    
    private bool _isCompiled = false;

    protected WebGLShader(IJSObjectReference jsModule, string vertexSource, string fragmentSource)
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

public class WebGLBasicColorShader : WebGLShader
{
    public WebGLBasicColorShader(IJSObjectReference jsModule) : base(jsModule, vertexSource, fragmentSource) { }

    private const string vertexSource = @"#version 300 es
        precision mediump float;
        in vec3 aPos;
        
        // Matrices
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;

        void main()
        {
            // Apply transformations in the correct order: Model -> View -> Projection
            vec4 worldPos = model * vec4(aPos, 1.0);
            vec4 clipSpacePos = projection * view * worldPos;
            gl_Position = clipSpacePos;
        }";

    private const string fragmentSource = @"#version 300 es
        precision mediump float;
        out vec4 FragColor;
        uniform vec4 uColor;
        void main() {
            FragColor = uColor;
        }";
}

public class WebGLSpriteShader : WebGLShader
{
    public WebGLSpriteShader(IJSObjectReference jsModule) : base(jsModule, vertexSource, fragmentSource) { }

    private const string vertexSource = @"#version 300 es
        precision mediump float;
        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec2 aTexCoord;
        uniform mat4 model;
        uniform mat4 projection;
        out vec2 TexCoord;
        void main() {
            gl_Position = projection * model * vec4(aPos, 1.0);
            TexCoord = aTexCoord;
        }";

    private const string fragmentSource = @"#version 300 es
        precision mediump float;
        in vec2 TexCoord;
        out vec4 FragColor;
        uniform sampler2D uTexture;
        void main() {
            FragColor = texture(uTexture, TexCoord);
        }";
}