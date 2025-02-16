namespace FireballEngine.Core.Utilities;

public abstract class LogFormatter
{
    public abstract string FormatMessage(MessageLevel level, string prefix, string message);
}

public class PlainLogFormatter : LogFormatter
{
    public override string FormatMessage(MessageLevel level, string prefix, string message)
    {
        return $"{prefix} {message}";
    }
}
