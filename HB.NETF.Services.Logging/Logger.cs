using HB.NETF.Services.Logging.Factory.Target;
using HB.NETF.Services.Logging.Exceptions;
using System.Runtime.CompilerServices;
using System.Text;
using System;
using System.IO;
using HB.NETF.Common.Extensions;
using HB.NETF.Common.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
        internal Logger(string category) : this() {
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

        private static IEnumerable<LogTarget> GetBySeverity(LogTarget[] logTargets, LogSeverity severity) {
            return logTargets.Where(e => {
                if (e.ValidSeverities.Length == 0)
                    return true;

                return e.ValidSeverities.Contains(severity);
            });
        }

        public void LogDebug(string message) {
            foreach (LogTarget target in GetBySeverity(LogTargets, LogSeverity.Debug))
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Debug, GetCurrentTime()));
        }

        public void LogTrace(string message) {
            foreach (LogTarget target in GetBySeverity(LogTargets, LogSeverity.Trace))
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Trace, GetCurrentTime()));
        }

        public void LogInformation(string message) {
            foreach (LogTarget target in GetBySeverity(LogTargets, LogSeverity.Information))
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Information, GetCurrentTime()));
        }

        public void LogWarning(string message) {
            foreach (LogTarget target in GetBySeverity(LogTargets, LogSeverity.Warning))
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Warning, GetCurrentTime()));
        }

        public void LogError(string message) {
            foreach (LogTarget target in GetBySeverity(LogTargets, LogSeverity.Error))
                LogInternal(target, new LogStatement(GetCategory(), message, LogSeverity.Error, GetCurrentTime()));
        }

        public void LogError(Exception exception) {
            LogError(exception.ToString());
        }

        public void LogCritical(string message) {
            foreach (LogTarget target in GetBySeverity(LogTargets, LogSeverity.Critical))
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