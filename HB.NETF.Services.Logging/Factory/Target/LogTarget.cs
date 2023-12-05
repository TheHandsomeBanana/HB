using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace HB.NETF.Services.Logging.Factory.Target {
    public readonly struct LogTarget {
        public object Target { get; }
        public LogSeverity MinLogSeverity { get; }

        public LogTarget(object target, LogSeverity minSeverity) {
            Target = target;
            MinLogSeverity = minSeverity;
            Validate(target);
        }

        public LogTarget(string file, LogSeverity minSeverity) {
            Target = file;
            MinLogSeverity = minSeverity;
        }

        public LogTarget(Stream stream, LogSeverity minSeverity) {
            Target = stream;
            MinLogSeverity = minSeverity;
        }

        public LogTarget(Action<LogStatement> action, LogSeverity minSeverity) : this((object)action, minSeverity) {
            Target = action;
            MinLogSeverity = minSeverity;
        }

        public LogTarget(Action<string> action, LogSeverity minSeverity) : this((object)action, minSeverity) {
            Target = action;
            MinLogSeverity = minSeverity;
        }

        private readonly static Type[] validTypes = new Type[] { typeof(string), typeof(Action<LogStatement>), typeof(Action<string>),
                typeof(FileStream), typeof(MemoryStream), typeof(CryptoStream) };
        private void Validate(object target) {
            Type runtimeTarget = target.GetType();
            if (!validTypes.Contains(runtimeTarget))
                throw new NotSupportedException($"Target type {runtimeTarget.FullName} is not supported.");
        }
    }
}
