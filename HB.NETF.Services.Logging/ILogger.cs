using System;

namespace HB.NETF.Services.Logging {
    public interface ILogger {
        void Log(string message, LogSeverity severity);
        void LogTrace(string message);
        void LogDebug(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception);
        void LogCritical(string message);
    }

    public interface ILogger<out T> : ILogger {
    }
}
