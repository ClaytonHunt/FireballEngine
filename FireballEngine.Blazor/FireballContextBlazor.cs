using System.Threading.Tasks;
using FireballEngine.Core;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace FireballEngine.Blazor;

public class FireballContextBlazor : IFireballContext
{
    public event UpdateHandler? OnUpdate;
    public event Action? OnRender;

    private readonly IJSObjectReference _jsModule;
    private double _previousTimestamp;
    private Color? _lastClearColor;

    public FireballContextBlazor(IJSObjectReference jsModule)
    {
        _jsModule = jsModule;
    }     

    public Task InitializeAsync(int width, int height, string title)
    {
        // Not used in Blazor, since we need a containerRef. We'll throw or no-op.
        throw new NotImplementedException("Use InitializeAsync overload with containerRef.");
    }

    public async Task InitializeAsync(DotNetObjectReference<FireballContextBlazor> dotNetRef, ElementReference containerRef, int width, int height, string title)
    {
        await _jsModule.InvokeVoidAsync("init", dotNetRef, containerRef, width, height, title);
    }

    [JSInvokable]
    public void OnFrame(double timestamp)
    {
        float deltaTime = (float)(timestamp - _previousTimestamp);
        _previousTimestamp = timestamp;

        OnUpdate?.Invoke(deltaTime);
        OnRender?.Invoke();
    }

    public async Task Clear(Color color)
    {
        if(!_lastClearColor.HasValue || !_lastClearColor.Value.Equals(color))
        {
            _lastClearColor = color;
            await _jsModule.InvokeVoidAsync("setClearColor", color.R, color.G, color.B, color.A);
        }

        await _jsModule.InvokeVoidAsync("clearBuffer");
    }
}