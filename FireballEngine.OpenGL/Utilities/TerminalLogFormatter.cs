using FireballEngine.Core.Utilities;

namespace FireballEngine.OpenGL.Utilities
{
    public class TerminalLogFormatter : LogFormatter
    {
        public override string FormatMessage(MessageLevel level, string prefix, string message)
        {
            string prefixColor = "\x1b[38;5;208m";
            string messageColor = level switch
            {
                MessageLevel.Info => "\x1b[38;5;39m",
                MessageLevel.Warning => "\x1b[38;5;226m",
                MessageLevel.Error => "\x1b[38;5;196m",
                _ => "\x1b[0m"
            };

            return $"{prefixColor}{prefix} {messageColor}{message}\x1b[0m";            
        }
    }
}