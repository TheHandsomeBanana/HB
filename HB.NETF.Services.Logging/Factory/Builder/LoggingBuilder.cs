using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.IO;

namespace HB.NETF.Services.Logging.Factory.Builder {
    internal class LoggingBuilder : ILoggingBuilder {
        private List<LogTarget> logTargets;
        public IReadOnlyList<LogTarget> LogTargets { get => logTargets; }

        public LoggingBuilder() {
            logTargets = new List<LogTarget>();
        }

        #region Target Handler

        public ILoggingBuilder AddTarget(object target, params LogSeverity[] severities) {
            LogTarget logTarget = new LogTarget(target);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Action<LogStatement> action, params LogSeverity[] severities) {
            LogTarget logTarget = new LogTarget(action);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Action<string> action, params LogSeverity[] severities) {
            LogTarget logTarget = new LogTarget(action);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(string file, params LogSeverity[] severities) {
            LogTarget logTarget = new LogTarget(file);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Stream stream, params LogSeverity[] severities) {
            LogTarget logTarget = new LogTarget(stream);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTargets(object[] targets, params LogSeverity[] severities) {
            foreach (object target in targets)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(Action<LogStatement>[] actions, params LogSeverity[] severities) {
            foreach (Action<LogStatement> target in actions)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(Action<string>[] actions, params LogSeverity[] severities) {
            foreach (Action<string> target in actions)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(string[] files, params LogSeverity[] severities) {
            foreach (string target in files)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(Stream[] streams, params LogSeverity[] severities) {
            foreach (Stream target in streams)
                AddTarget(target);

            return this;
        }
        #endregion
    }
}
