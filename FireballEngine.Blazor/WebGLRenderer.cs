using FireballEngine.Core;
using FireballEngine.Core.Assets;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor;

public class WebGLRenderer : Renderer
{
    private readonly IJSObjectReference _jsModule;

    public WebGLRenderer(IJSObjectReference jsModule)
    {
        _jsModule = jsModule;
    }

    public override async void DrawSprite(Texture2D texture, Material material, float x, float y, float width, float height, SpriteOrigin origin = SpriteOrigin.Center, float customOriginX = 0.5f, float customOriginY = 0.5f)
    {
        material.Use();
        texture.Bind();

        (float originX, float originY) = GetOriginOffset(origin, customOriginX, customOriginY);

        await _jsModule.InvokeVoidAsync("drawSprite", texture.Name, x, y, width, height, originX, originY);
    }

    public override async void DrawTriangle()
    {
        await _jsModule.InvokeVoidAsync("drawTriangle");
    }

    private (float, float) GetOriginOffset(SpriteOrigin origin, float customX, float customY)
    {
        switch (origin)
        {
            case SpriteOrigin.TopLeft: return (0.0f, 0.0f);
            case SpriteOrigin.TopCenter: return (0.5f, 0.0f);
            case SpriteOrigin.TopRight: return (1.0f, 0.0f);
            case SpriteOrigin.MiddleLeft: return (0.0f, 0.5f);
            case SpriteOrigin.Center: return (0.5f, 0.5f);
            case SpriteOrigin.MiddleRight: return (1.0f, 0.5f);
            case SpriteOrigin.BottomLeft: return (0.0f, 1.0f);
            case SpriteOrigin.BottomCenter: return (0.5f, 1.0f);
            case SpriteOrigin.BottomRight: return (1.0f, 1.0f);
            case SpriteOrigin.Custom: return (customX, customY);
            default: return (0.5f, 0.5f); // Default to center
        }
    }
}