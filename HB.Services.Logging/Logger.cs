using HB.Services.Logging.Factory.Target;
using HB.Services.Logging.Exceptions;
using System.Runtime.CompilerServices;
using System.Text;

namespace HB.Services.Logging {
    internal class Logger<T> : Logger, ILogger<T> where T : new() {
        public Logger() : base() {
            Type t = typeof(T);
            if (t.FullName == null)
                throw new LoggerException($"Type {t.Name} is invalid.");

            GenericType = t;
        }
    }

    internal class Logger : ILogger {
        internal LogTarget[] LogTargets { get; set; }
        public Type GenericType { get; set; }
        public DateTimeKind TimeKind { get; private set; } = DateTimeKind.Local;

        protected Logger() {
            LogTargets = new LogTarget[0];
        }
        internal Logger(Type t) {
            if (t.FullName == null)
                throw new LoggerException($"Type {t.Name} is invalid.");

            GenericType = t;
            LogTargets = new LogTarget[0];
        }

        public void LogDebug(string message) {
            LogInternal(new LogStatement(GenericType.FullName, message, LogSeverity.Debug, GetCurrentTime()));
        }

        public void LogTrace(string message) {
            LogInternal(new LogStatement(GenericType.FullName, message, LogSeverity.Trace, GetCurrentTime()));
        }

        public void LogInformation(string message) {
            LogInternal(new LogStatement(GenericType.FullName, message, LogSeverity.Information, GetCurrentTime()));
        }

        public void LogWarning(string message) {
            LogInternal(new LogStatement(GenericType.FullName, message, LogSeverity.Warning, GetCurrentTime()));
        }

        public void LogError(string message) {
            LogInternal(new LogStatement(GenericType.FullName, message, LogSeverity.Error, GetCurrentTime()));
        }

        public void LogCritical(string message) {
            LogInternal(new LogStatement(GenericType.FullName, message, LogSeverity.Critical, GetCurrentTime()));
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
        private void HandleString(string str, LogStatement log) {
            using (StreamWriter sw = new StreamWriter(str, true))
                sw.WriteLine(log.ToString());
        }
        private void HandleStream(Stream stream, LogStatement log) {
            stream.Write(Encoding.UTF8.GetBytes(log.ToString() + "\r"));
            stream.Position = 0;
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
        #endregion
    }
}