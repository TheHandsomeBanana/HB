using HB.Services.Logging.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Services.Logging.Factory.Builder;
using HB.Services.Logging;
using HB.Services.Logging.Factory.Target;
using HB.Common.Enumerators;

namespace HB.Services.Logging.Factory {
    public class LoggerFactory : ILoggerFactory {
        private List<string> capturedLoggerCategories;
        public IReadOnlyList<string> CapturedLoggerCategories { get => capturedLoggerCategories; }
        public LogTarget[] GlobalLogTargets { get; }

        public LoggerFactory() {
            capturedLoggerCategories = new List<string>();
            GlobalLogTargets = new LogTarget[0];
        }

        public LoggerFactory(params object[] globalTargets) : this() {
            GlobalLogTargets = new LogTarget[globalTargets.Length];
            foreach (int i in globalTargets.Length)
                GlobalLogTargets[i] = new LogTarget(globalTargets[i]);
        }

        public ILogger CreateLogger(string? category, Action<ILoggingBuilder> builder) {
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
    }
}
