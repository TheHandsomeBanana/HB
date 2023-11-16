using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Logging.Factory.Target {
    public readonly struct LogTarget {
        public object Target { get; }
        public LogSeverity[] ValidSeverities { get; }
        
        public LogTarget(object target, params LogSeverity[] severities) {
            Target = target;
            ValidSeverities = severities;
            Validate(target);
        }

        public LogTarget(string file, params LogSeverity[] severities) {
            Target = file;
            ValidSeverities = severities;
        }

        public LogTarget(Stream stream, params LogSeverity[] severities) {
            Target = stream;
            ValidSeverities = severities;
        }

        public LogTarget(Action<LogStatement> action, params LogSeverity[] severities) : this((object)action, severities) {
            Target = action;
            ValidSeverities = severities;
        }

        public LogTarget(Action<string> action, params LogSeverity[] severities) : this((object)action, severities) {
            Target = action;
            ValidSeverities = severities;
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
