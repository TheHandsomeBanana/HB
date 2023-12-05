using HB.NETF.Common.Extensions;
using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HB.NETF.Services.Logging {
    internal class Logger<T> : Logger, ILogger<T> {
        public Logger() {
            Category = typeof(T).Name;
        }
    }

    internal class Logger : ILogger {
        internal LogTarget[] TraceTargets { get; set; } = Array.Empty<LogTarget>();
        internal LogTarget[] DebugTargets { get; set; } = Array.Empty<LogTarget>();
        internal LogTarget[] InformationTargets { get; set; } = Array.Empty<LogTarget>();
        internal LogTarget[] WarningTargets { get; set; } = Array.Empty<LogTarget>();
        internal LogTarget[] ErrorTargets { get; set; } = Array.Empty<LogTarget>();
        internal LogTarget[] CriticalTargets { get; set; } = Array.Empty<LogTarget>();

        public string Category { get; set; }
        public DateTimeKind TimeKind { get; private set; } = DateTimeKind.Local;
        protected Logger() { }

        internal Logger(string category) {
            Category = category;
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
            foreach (LogTarget target in DebugTargets)
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Debug, GetCurrentTime()));
        }

        public void LogTrace(string message) {
            foreach (LogTarget target in TraceTargets)
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Trace, GetCurrentTime()));
        }

        public void LogInformation(string message) {
            foreach (LogTarget target in InformationTargets)
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Information, GetCurrentTime()));
        }

        public void LogWarning(string message) {
            foreach (LogTarget target in WarningTargets)
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Warning, GetCurrentTime()));
        }

        public void LogError(string message) {
            foreach (LogTarget target in ErrorTargets)
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Error, GetCurrentTime()));
        }

        public void LogError(Exception exception) {
            LogError(exception.ToString());
        }

        public void LogCritical(string message) {
            foreach (LogTarget target in CriticalTargets)
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Critical, GetCurrentTime()));
        }

        private void LogInternal(LogTarget t, LogStatement log) {
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

        #region LogHelper
        private readonly static object lockObj = new object();
        private void HandleString(string str, LogStatement log) {
            lock (lockObj) {
                using (StreamWriter sw = new StreamWriter(str, true))
                    sw.WriteLine(log.ToString());
            }
        }
        private void HandleStream(Stream stream, LogStatement log) {
            lock (lockObj) {
                stream.Write(Encoding.UTF8.GetBytes(log.ToString() + "\r"));
            }
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