using HB.NETF.Services.Logging.Exceptions;
using HB.NETF.Services.Logging.Factory.Builder;
using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HB.NETF.Services.Logging.Factory {
    public class LoggerFactory : ILoggerFactory {
        public LogTarget[] GlobalLogTargets { get; private set; }
        public Dictionary<string, ILogger> LoggerContainer { get; set; } = new Dictionary<string, ILogger>();

        public LoggerFactory() {
            GlobalLogTargets = new LogTarget[0];
        }

        public void InvokeLoggingBuilder(Action<ILoggingBuilder> builder) {
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

            return CreateLogger(category, loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray());
        }

        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder) {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (LoggerContainer.ContainsKey(typeof(T).FullName))
                throw new LoggerException($"{typeof(T).FullName} already exists.");

            LoggingBuilder loggingBuilder = new LoggingBuilder();
            builder.Invoke(loggingBuilder);

            return CreateLogger<T>(loggingBuilder.LogTargets.Concat(GlobalLogTargets).ToArray());
        }

        public ILogger GetOrCreateLogger(string category) {
            if (LoggerContainer.ContainsKey(category))
                return LoggerContainer[category];

            return CreateLogger(category, GlobalLogTargets);
        }

        public ILogger<T> GetOrCreateLogger<T>() {
            if (LoggerContainer.ContainsKey(typeof(T).FullName))
                return (ILogger<T>)LoggerContainer[typeof(T).FullName];

            Logger<T> logger = new Logger<T>();
            logger.TraceTargets = FilterLogTargets(GlobalLogTargets, LogSeverity.Trace);
            logger.DebugTargets = FilterLogTargets(GlobalLogTargets, LogSeverity.Debug);
            logger.InformationTargets = FilterLogTargets(GlobalLogTargets, LogSeverity.Information);
            logger.WarningTargets = FilterLogTargets(GlobalLogTargets, LogSeverity.Warning);
            logger.ErrorTargets = FilterLogTargets(GlobalLogTargets, LogSeverity.Error);
            logger.CriticalTargets = FilterLogTargets(GlobalLogTargets, LogSeverity.Critical);
            this.LoggerContainer.Add(typeof(T).FullName, logger);
            return logger;
        }

        private Logger CreateLogger(string category, LogTarget[] logTargets) {
            Logger logger = new Logger(category);
            logger.TraceTargets = FilterLogTargets(logTargets, LogSeverity.Trace);
            logger.DebugTargets = FilterLogTargets(logTargets, LogSeverity.Debug);
            logger.InformationTargets = FilterLogTargets(logTargets, LogSeverity.Information);
            logger.WarningTargets = FilterLogTargets(logTargets, LogSeverity.Warning);
            logger.ErrorTargets = FilterLogTargets(logTargets, LogSeverity.Error);
            logger.CriticalTargets = FilterLogTargets(logTargets, LogSeverity.Critical);

            this.LoggerContainer.Add(category, logger);
            return logger;
        }

        private Logger<T> CreateLogger<T>(LogTarget[] logTargets) {
            Logger<T> logger = new Logger<T>();
            logger.TraceTargets = FilterLogTargets(logTargets, LogSeverity.Trace);
            logger.DebugTargets = FilterLogTargets(logTargets, LogSeverity.Debug);
            logger.InformationTargets = FilterLogTargets(logTargets, LogSeverity.Information);
            logger.WarningTargets = FilterLogTargets(logTargets, LogSeverity.Warning);
            logger.ErrorTargets = FilterLogTargets(logTargets, LogSeverity.Error);
            logger.CriticalTargets = FilterLogTargets(logTargets, LogSeverity.Critical);
            this.LoggerContainer.Add(typeof(T).FullName, logger);
            return logger;
        }

        private LogTarget[] FilterLogTargets(LogTarget[] logTargets, LogSeverity severity) => logTargets.Where(e => e.MinLogSeverity <= severity).ToArray();
    }
}
