
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
//using FoucsBI.Dashboard.Models;

namespace SCS.Dashboard
{
    public class ApplicationLogging : IApplicationLogging
    {
        #region Debugging Log

        public void LogInfo(string message)
        {
            Log(message, LogLevel.Info);
        }

        public void LogDebug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }

        public void LogFatal(string message)
        {
            Log(message, LogLevel.Fatal);
        }

        public void LogWarn(string message)
        {
            Log(message, LogLevel.Warn);
        }

        public void Log(string message, LogLevel level)
        {
            AppLogger.WriteLogEntry(message, level);
        }

        #endregion
    }
}