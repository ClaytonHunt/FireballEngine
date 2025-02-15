using System.Threading.Tasks;
using FireballEngine.Core.Graphics;
using Microsoft.JSInterop;

namespace FireballEngine.Blazor
{
    public class WebGLTexture2D : Texture2D
    {
        private readonly IJSObjectReference _jsModule;

        public WebGLTexture2D(int width, int height, int textureId, IJSObjectReference jsModule) : base(width, height, textureId)
        {
            _jsModule = jsModule;
        }

        public override async void Bind()
        {
            await _jsModule.InvokeVoidAsync("bindTexture", _textureId);
        }
    }
}