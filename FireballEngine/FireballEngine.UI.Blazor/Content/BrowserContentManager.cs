using FireballEngine.Core.Content;
using Microsoft.JSInterop;

namespace FireballEngine.UI.Blazor.Content
{
    public class BrowserContentManager : IContentManager
    {
        private readonly IJSObjectReference _jsModule;

        public BrowserContentManager(IJSObjectReference jsModule)
        {
            _jsModule = jsModule;
        }

        public async Task Register<T>(string label, string source)
        {
            await _jsModule.InvokeAsync<T>("registerContent", label, source);
        }
    }
}
