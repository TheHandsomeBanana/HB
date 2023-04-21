using HB.NETF.Services.Logging.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.NETF.Services.Logging.Factory.Builder;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory.Target;

namespace HB.NETF.Services.Logging.Factory {
    public class LoggerFactory : ILoggerFactory {
        private List<Type> capturedLoggerTypes;
        public IReadOnlyList<Type> CapturedLoggerTypes { get => capturedLoggerTypes; }
        public LogTarget[] GlobalLogTargets { get; }

        public LoggerFactory() {
            capturedLoggerTypes = new List<Type>();
            GlobalLogTargets = new LogTarget[0];
        }

        public LoggerFactory(params object[] globalTargets) : this() {
            GlobalLogTargets = new LogTarget[globalTargets.Length];
            for(int i = 0; i < globalTargets.Length; i++)
                GlobalLogTargets[i] = new LogTarget(globalTargets[i]);
        }

        public ILogger CreateLogger(Type loggerType, Action<ILoggingBuilder> builder) {
            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            Logger logger = new Logger(loggerType);
            logger.LogTargets = loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray();

            capturedLoggerTypes.Add(loggerType);
            return logger;
        }

        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder) where T : new() {
            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            Logger<T> logger = new Logger<T>();
            logger.LogTargets = loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray();

            capturedLoggerTypes.Add(typeof(T));
            return logger;
        }
    }
}
