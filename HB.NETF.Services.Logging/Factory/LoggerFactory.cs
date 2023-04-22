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
        private List<string> capturedLoggerCategories;
        public LogTarget[] GlobalLogTargets { get; }
        public IReadOnlyList<string> CapturedLoggerCategories => capturedLoggerCategories;

        public LoggerFactory() {
            capturedLoggerCategories = new List<string>();
            GlobalLogTargets = new LogTarget[0];
        }

        /// <summary>
        /// Adds Global Log Targets to factory (each logger will write to these targets)
        /// </summary>
        /// <param name="builder"></param>
        public LoggerFactory(Action<ILoggingBuilder> builder) {
            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            GlobalLogTargets = loggingBuilder.LogTargets.ToArray();
        }

        public ILogger CreateLogger(string category, Action<ILoggingBuilder> builder) {
            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            Logger logger = new Logger(category);
            logger.LogTargets = loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray();

            if (category != null)
                capturedLoggerCategories.Add(category);

            return logger;
        }

        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder) where T : new() {
            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            Logger<T> logger = new Logger<T>();
            logger.LogTargets = loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray();

            capturedLoggerCategories.Add(nameof(T));
            return logger;
        }

        public ILogger CreateLogger(Type loggerType, Action<ILoggingBuilder> builder) {
            throw new NotImplementedException();
        }
    }
}
