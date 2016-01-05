using NLog;

namespace SCS.Dashboard
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public static class AppLogger 
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void WriteLogEntry(string message, LogLevel level)
        {
            WriteLogEntry(message, null, level);
        }

        public static void WriteLogEntry(string message, System.Exception ex, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    logger.Debug(ex, message);
                    break;
                case LogLevel.Error:
                    logger.Error(ex, message);
                    break;
                case LogLevel.Info:
                    logger.Info(ex, message);
                    break;
                case LogLevel.Fatal:
                    logger.Fatal(ex, message);
                    break;
                case LogLevel.Warn:
                    logger.Warn(ex, message);
                    break;
            }
        }
    }   
}