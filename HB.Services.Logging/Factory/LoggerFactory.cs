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
        public LogTarget[] GlobalLogTargets { get; } = Array.Empty<LogTarget>();
        public Dictionary<string, ILogger> LoggerContainer { get; set; } = new Dictionary<string, ILogger>();

        public LoggerFactory() { 
        }

        /// <summary>
        /// Adds Global Log Targets to factory (each logger will write to these targets)
        /// </summary>
        /// <param name="builder"></param>
        public LoggerFactory(Action<ILoggingBuilder> builder) {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            GlobalLogTargets = loggingBuilder.LogTargets.ToArray();
        }

        public ILogger CreateLogger(string category, Action<ILoggingBuilder> builder) {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            if (LoggerContainer.ContainsKey(category))
                throw new LoggerException($"{category} already exists.");

            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            Logger logger = new Logger(category);
            logger.LogTargets = loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray();

            LoggerContainer.Add(category, logger);
            return logger;
        }

        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder) {
            if (typeof(T).FullName is null)
                throw new LoggerException($"Generic type not valid as category");

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (LoggerContainer.ContainsKey(typeof(T).FullName))
                throw new LoggerException($"{typeof(T).FullName} already exists.");

            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            Logger<T> logger = new Logger<T>();
            logger.LogTargets = loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray();
            this.LoggerContainer.Add(typeof(T).FullName, logger);
            return logger;
        }

        public ILogger GetOrCreateLogger(string category) {
            if (LoggerContainer.ContainsKey(category))
                return LoggerContainer[category];

            Logger logger = new Logger(category);
            logger.LogTargets = GlobalLogTargets;
            this.LoggerContainer.Add(category, logger);
            return logger;
        }

        public ILogger<T> GetOrCreateLogger<T>() {
            if (typeof(T).FullName is null)
                throw new LoggerException($"Generic type not valid as category");

            if (LoggerContainer.ContainsKey(typeof(T).FullName))
                return (ILogger<T>)LoggerContainer[typeof(T).FullName];

            Logger<T> logger = new Logger<T>();
            logger.LogTargets = GlobalLogTargets;

            this.LoggerContainer.Add(typeof(T).FullName, logger);
            return logger;
        }
    }
}
