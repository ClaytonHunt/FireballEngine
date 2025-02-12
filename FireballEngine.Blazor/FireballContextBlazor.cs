using FireballEngine.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor
{
    public class FireballContextBlazor : IFireballContext
    {
        private readonly IJSObjectReference _jsModule;
        private double _previousTimestamp;
        private Color? _lastClearColor;

        public IGame Game { get; }

        public FireballContextBlazor(IJSObjectReference jsModule, IGame game)
        {
            _jsModule = jsModule;
            Game = game;
        }

        public Task InitializeAsync(int width, int height, string title)
        {
            throw new NotImplementedException("Use the specialized overload for Blazor.");
        }

        public async Task InitializeAsync(
            DotNetObjectReference<FireballContextBlazor> dotNetRef,
            ElementReference containerRef,
            int width,
            int height,
            string title)
        {
            await _jsModule.InvokeVoidAsync("init", dotNetRef, containerRef, width, height, title);
        }

        [JSInvokable]
        public void OnFrame(double timestamp)
        {
            float deltaMs = (float)(timestamp - _previousTimestamp);
            _previousTimestamp = timestamp;
            Game.Update(deltaMs);
            Game.Render(this);
        }

        public async Task Clear(Color color)
        {
            if (!_lastClearColor.HasValue || !_lastClearColor.Value.Equals(color))
            {
                _lastClearColor = color;
                await _jsModule.InvokeVoidAsync("setClearColor", color.R, color.G, color.B, color.A);
            }

            await _jsModule.InvokeVoidAsync("clearBuffer");
        }
    }
}
