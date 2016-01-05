using System;
namespace SCS.Dashboard
{
    public interface IApplicationLogging
    {
        void LogInfo(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogFatal(string message);
        void LogWarn(string message);
        void Log(string message, LogLevel level);

    }
}
