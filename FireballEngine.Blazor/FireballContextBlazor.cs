using FireballEngine.Blazor.Assets;
using FireballEngine.Core;
using FireballEngine.Core.Assets;
using FireballEngine.Core.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor
{
    public class FireballContextBlazor : IFireballContext
    {
        private IJSObjectReference _jsModule;
        private double _previousTimestamp;       

        public IGame Game { get; }

        public IInput Input { get; private set; } = null!;

        public IAssetManager AssetManager { get; }

        public Renderer Renderer { get; }

        public FireballContextBlazor(IJSObjectReference jsModule, IGame game)
        {            
            _jsModule = jsModule;
            Game = game;
            AssetManager = new WebGLAssetManager(_jsModule);     
            Renderer = new WebGLRenderer(_jsModule);
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
            Input = new BlazorInput();
            
            await _jsModule.InvokeVoidAsync("init", dotNetRef, containerRef, width, height, title);            

            await Game.OnLoad(this);

            await _jsModule.InvokeVoidAsync("start");
        }

        [JSInvokable]
        public void OnFrame(double timestamp)
        {
            float deltaMs = (float)(timestamp - _previousTimestamp);
            _previousTimestamp = timestamp;
            Game.Update(deltaMs);
            Game.Render();
        }

        [JSInvokable]
        public void LogMessage(string level, string message)
        {
            switch (level.ToLower())
            {
                case "info":
                    Fire.Info(message);
                    break;
                case "warn":
                    Fire.Warning(message);
                    break;
                case "error":
                    Fire.Error(message);
                    break;
                default:
                    Fire.Info(message);
                    break;
            }
        }

        /// <summary>
        /// Factory method to create a platform-specific shader.
        /// </summary>
        /// <param name="vertexSource"></param>
        /// <param name="fragmentSource"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Shader CreateShader(ShaderType type)
        {
            switch (type)
            {
                case ShaderType.BasicColor:
                    return new WebGLBasicColorShader(_jsModule);
                case ShaderType.Sprite:
                    return new WebGLSpriteShader(_jsModule);
                default:
                    throw new NotImplementedException("Unknown shader type.");
            }
        }        
    }
}
