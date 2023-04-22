using HB.Services.Logging.Factory.Builder;
using HB.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Logging.Factory {
    public interface ILoggerFactory {
        public IReadOnlyList<string> CapturedLoggerCategories { get; }
        public LogTarget[] GlobalLogTargets { get; }
        public ILogger CreateLogger(string category, Action<ILoggingBuilder> builder);
        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder) where T : new();
    }
}
