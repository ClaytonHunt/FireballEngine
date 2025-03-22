using FireballEngine.Core.Utils;
using Microsoft.JSInterop;
using System.Text;

namespace FireballEngine.Blazor.Utilities;

public class BlazorConsoleWriter : TextWriter
{
    private readonly IJSRuntime _jsRuntime;

    public BlazorConsoleWriter(IJSRuntime jsRuntime, MessageLevel logLevel)
    {
        _jsRuntime = jsRuntime;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void WriteLine(string? value)
    {
        var logLevel = value?.Substring(0, value.IndexOf(':') + 1);
        var message = value?.Substring(value.IndexOf(':') + 1);

        // convert logLevel to MessageLevel
        var level = logLevel switch
        {
            "info:" => MessageLevel.Info,
            "warn:" => MessageLevel.Warning,
            "error:" => MessageLevel.Error,
            _ => MessageLevel.Info
        };

        LogToConsole(level, message);
    }

    public override void WriteLine(string format, params object?[] arg)
    {           
        LogToConsole(MessageLevel.Info, string.Format(format, arg));
    }

    private void LogToConsole(MessageLevel level, string? message)
    {
        if (message != null)
        {
            var logLevel = level switch
            {
                MessageLevel.Info => "log",
                MessageLevel.Warning => "warn",
                MessageLevel.Error => "error",
                _ => "log"
            };

            string[] styles = BrowserConsoleLogFormatter.GetStyles(level);

            _jsRuntime.InvokeVoidAsync($"console.{logLevel}", new object[] { message }.Concat(styles).ToArray());
        }
    }
}