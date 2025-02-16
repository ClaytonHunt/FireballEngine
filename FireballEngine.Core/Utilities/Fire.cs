namespace FireballEngine.Core.Utilities;

public enum MessageLevel
{
    Info,
    Warning,
    Error
}

public static class Fire
{
    private static LogFormatter _formatter = new PlainLogFormatter();
    private const string FireballPrefix = "[Fireball]";

    public static void SetFormatter(LogFormatter formatter)
    {
        _formatter = formatter;
    }

    public static void Info(string message)
    {
        Console.Out.WriteLine(_formatter.FormatMessage(MessageLevel.Info, FireballPrefix, message));
    }

    public static void Warning(string message)
    {
        Console.Out.WriteLine(_formatter.FormatMessage(MessageLevel.Warning, FireballPrefix, message));
    }

    public static void Error(string message)
    {
        Console.Error.WriteLine(_formatter.FormatMessage(MessageLevel.Error, FireballPrefix, message));
    }
}