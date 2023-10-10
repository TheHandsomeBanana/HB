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
        public LogTarget[] GlobalLogTargets { get; }
        public Dictionary<string, ILogger> LoggerContainer { get; set; } = new Dictionary<string, ILogger>();
 
        public LoggerFactory() {
            GlobalLogTargets = new LogTarget[0];
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
            if(category is null)
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

        public ILogger<T> CreateLogger<T>(Action<ILoggingBuilder> builder){
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
            if(LoggerContainer.ContainsKey(category))
                return LoggerContainer[category];

            Logger logger = new Logger(category);
            logger.LogTargets = GlobalLogTargets;
            this.LoggerContainer.Add(category, logger);
            return logger;
        }

        public ILogger<T> GetOrCreateLogger<T>() {
            if(LoggerContainer.ContainsKey(typeof(T).FullName))
                return (ILogger<T>)LoggerContainer[typeof(T).FullName];

            Logger<T> logger = new Logger<T>();
            logger.LogTargets = GlobalLogTargets;

            this.LoggerContainer.Add(typeof(T).FullName, logger);
            return logger;
        }
    }
}
