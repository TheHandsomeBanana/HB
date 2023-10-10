using HB.Services.Logging.Factory.Builder;
using HB.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Logging.Factory {
    public interface ILoggerFactory {
        public LogTarget[] GlobalLogTargets { get; }
        public ILogger GetOrCreateLogger(string category);
        public ILogger<T> GetOrCreateLogger<T>();
        public ILogger CreateLogger(string category, Action<ILoggingBuilder> builder);
        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder);
    }
}
