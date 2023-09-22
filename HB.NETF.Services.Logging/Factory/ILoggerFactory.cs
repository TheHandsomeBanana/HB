using HB.NETF.Services.Logging.Factory.Builder;
using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Logging.Factory {
    public interface ILoggerFactory {
        LogTarget[] GlobalLogTargets { get; }
        ILogger GetOrCreateLogger(string category);
        ILogger<T> GetOrCreateLogger<T>();
        ILogger CreateLogger(string category, Action<ILoggingBuilder> builder);
        ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder);
    }
}
