using HB.NETF.Services.Logging.Factory.Builder;
using HB.NETF.Services.Logging.Factory.Target;
using System;

namespace HB.NETF.Services.Logging.Factory {
    public interface ILoggerFactory {
        LogTarget[] GlobalLogTargets { get; }
        void InvokeLoggingBuilder(Action<ILoggingBuilder> builder);
        ILogger GetOrCreateLogger(string category);
        ILogger<T> GetOrCreateLogger<T>();
        ILogger CreateLogger(string category, Action<ILoggingBuilder> builder);
        ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder);
    }
}
