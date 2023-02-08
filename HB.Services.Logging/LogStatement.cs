using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Logging {
    public struct LogStatement {
        public string BoundTypeName { get; }
        public string Message { get; }
        public LogSeverity Severity { get; }
        public DateTime Timestamp { get; }

        public LogStatement(string boundTypeName, string message, LogSeverity severity, DateTime timestamp) {
            BoundTypeName = boundTypeName;
            Message = message;
            Severity = severity;
            Timestamp = timestamp;
        }

        public override string ToString() {
            return $"[{BoundTypeName}] [{Timestamp.ToString("dd:MM:yyyy HH:mm:ss")}] [{Severity}]: {Message}";
        }
    }

    public enum LogSeverity {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical,
    }
}
