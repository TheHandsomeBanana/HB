using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Logging {
    public interface ILogger {
        public void LogTrace(string message);
        public void LogDebug(string message);
        public void LogInformation(string message);
        public void LogWarning(string message);
        public void LogError(string message);
        public void LogCritical(string message);
    }

    public interface ILogger<out T> : ILogger where T : new() {
        public string Category { get; }
    }
}
