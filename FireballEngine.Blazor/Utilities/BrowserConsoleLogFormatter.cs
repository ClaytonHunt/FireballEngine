using FireballEngine.Core.Utilities;

namespace FireballEngine.Blazor.Utilities;

public class BrowserConsoleLogFormatter : LogFormatter
{
    public override string FormatMessage(MessageLevel level, string prefix, string message)
    {
        var logLevelPrefix = level switch
        {
            MessageLevel.Info => "info",
            MessageLevel.Warning => "warn",
            MessageLevel.Error => "error",
            _ => "info"
        };

        return $"{logLevelPrefix}:%c{prefix} %c{message}";
    }

    public static string[] GetStyles(MessageLevel level)
    {
        return
        [
            "color: orange; font-weight: bold;", // Style for prefix
            level switch
            {
                MessageLevel.Info => "color: lightblue;",
                MessageLevel.Warning => "color: yellow;",
                MessageLevel.Error => "color: red;",
                _ => "color: white;"
            } // Style for message
        ];
    }
}
