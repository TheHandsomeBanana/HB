using HB.NETF.Services.Logging.Factory.Target;
using HB.NETF.Services.Logging.Exceptions;
using System.Runtime.CompilerServices;
using System.Text;
using System;
using System.IO;
using HB.NETF.Common.Extensions;

namespace HB.NETF.Services.Logging {
    internal class Logger<T> : Logger, ILogger<T> {
        public Logger() : base() {
            Category = typeof(T).Name;
        }
    }

    internal class Logger : ILogger {
        internal LogTarget[] LogTargets { get; set; }
        public string Category { get; set; }
        public DateTimeKind TimeKind { get; private set; } = DateTimeKind.Local;

        protected Logger() {
            LogTargets = new LogTarget[0];
        }
        internal Logger(string category) {
            Category = category;
            LogTargets = new LogTarget[0];
        }

        public void Log(string message, LogSeverity severity) {
            switch (severity) {
                case LogSeverity.Trace:
                    LogTrace(message);
                    break;
                case LogSeverity.Debug:
                    LogDebug(message);
                    break;
                case LogSeverity.Information:
                    LogInformation(message);
                    break;
                case LogSeverity.Warning:
                    LogWarning(message);
                    break;
                case LogSeverity.Error:
                    LogError(message);
                    break;
                case LogSeverity.Critical:
                    LogCritical(message);
                    break;
            }
        }

        public void LogDebug(string message) {
            LogInternal(new LogStatement(GetCategory(), message, LogSeverity.Debug, GetCurrentTime()));
        }

        public void LogTrace(string message) {
            LogInternal(new LogStatement(GetCategory(), message, LogSeverity.Trace, GetCurrentTime()));
        }

        public void LogInformation(string message) {
            LogInternal(new LogStatement(GetCategory(), message, LogSeverity.Information, GetCurrentTime()));
        }

        public void LogWarning(string message) {
            LogInternal(new LogStatement(GetCategory(), message, LogSeverity.Warning, GetCurrentTime()));
        }

        public void LogError(string message) {
            LogInternal(new LogStatement(GetCategory(), message, LogSeverity.Error, GetCurrentTime()));
        }

        public void LogCritical(string message) {
            LogInternal(new LogStatement(GetCategory(), message, LogSeverity.Critical, GetCurrentTime()));
        }

        private void LogInternal(LogStatement log) {
            foreach (LogTarget t in LogTargets) {
                switch (t.Target) {
                    case Action<LogStatement> logAction:
                        logAction.Invoke(log);
                        break;
                    case Action<string> stringAction:
                        stringAction.Invoke(log.ToString());
                        break;
                    case string str:
                        HandleString(str, log);
                        break;
                    case Stream stream:
                        HandleStream(stream, log);
                        break;
                }
            }
        }

        #region LogHelper
        private static object lockObject = new object();
        private void HandleString(string str, LogStatement log) {
            lock (lockObject) {
                using (StreamWriter sw = new StreamWriter(str, true))
                    sw.WriteLine(log.ToString());
            }
        }
        private void HandleStream(Stream stream, LogStatement log) {
            stream.Write(Encoding.UTF8.GetBytes(log.ToString() + "\r"));
        }
        private DateTime GetCurrentTime() {
            switch (TimeKind) {
                case DateTimeKind.Utc:
                    return DateTime.UtcNow;
                case DateTimeKind.Local:
                    return DateTime.Now;
                default:
                    return DateTime.Now;
            }
        }

        private string GetCategory() => Category ?? "No Category";
        #endregion
    }
}