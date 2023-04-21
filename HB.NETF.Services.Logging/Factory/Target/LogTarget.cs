using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Logging.Factory.Target {
    public struct LogTarget {
        public object Target { get; }
        public LogTarget(object target) {
            Target = target;
            Validate(target);
        }

        public LogTarget(string file) {
            Target = file;
        }

        public LogTarget(Stream stream) {
            Target = stream;
        }

        public LogTarget(Action<LogStatement> action) {
            Target = action;
        }

        public LogTarget(Action<string> action) {
            Target = action;
        }

        private void Validate(object target) {
            Type runtimeTarget = target.GetType();
            Type[] validTypes = new Type[] { typeof(string), typeof(Action<LogStatement>), typeof(Action<string>),
                typeof(FileStream), typeof(MemoryStream), typeof(CryptoStream) };
            if (!validTypes.Contains(runtimeTarget))
                throw new NotSupportedException($"Target type {runtimeTarget.FullName} is not supported.");
        }
    }
}
