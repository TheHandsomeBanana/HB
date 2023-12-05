﻿using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.IO;

namespace HB.NETF.Services.Logging.Factory.Builder {
    internal class LoggingBuilder : ILoggingBuilder {
        private readonly List<LogTarget> logTargets;
        public IReadOnlyList<LogTarget> LogTargets { get => logTargets; }

        public LoggingBuilder() {
            logTargets = new List<LogTarget>();
        }

        #region Target Handler

        public ILoggingBuilder AddTarget(object target, LogSeverity minSeverity = LogSeverity.Trace) {
            LogTarget logTarget = new LogTarget(target, minSeverity);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Action<LogStatement> action, LogSeverity minSeverity = LogSeverity.Trace) {
            LogTarget logTarget = new LogTarget(action, minSeverity);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Action<string> action, LogSeverity minSeverity = LogSeverity.Trace) {
            LogTarget logTarget = new LogTarget(action, minSeverity);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(string file, LogSeverity minSeverity = LogSeverity.Trace) {
            LogTarget logTarget = new LogTarget(file, minSeverity);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Stream stream, LogSeverity minSeverity = LogSeverity.Trace) {
            LogTarget logTarget = new LogTarget(stream, minSeverity);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTargets(object[] targets, LogSeverity minSeverity = LogSeverity.Trace) {
            foreach (object target in targets)
                AddTarget(target, minSeverity);

            return this;
        }

        public ILoggingBuilder AddTargets(Action<LogStatement>[] actions, LogSeverity minSeverity = LogSeverity.Trace) {
            foreach (Action<LogStatement> target in actions)
                AddTarget(target, minSeverity);

            return this;
        }

        public ILoggingBuilder AddTargets(Action<string>[] actions, LogSeverity minSeverity = LogSeverity.Trace) {
            foreach (Action<string> target in actions)
                AddTarget(target, minSeverity);

            return this;
        }

        public ILoggingBuilder AddTargets(string[] files, LogSeverity minSeverity = LogSeverity.Trace) {
            foreach (string target in files)
                AddTarget(target, minSeverity);

            return this;
        }

        public ILoggingBuilder AddTargets(Stream[] streams, LogSeverity minSeverity = LogSeverity.Trace) {
            foreach (Stream target in streams)
                AddTarget(target, minSeverity);

            return this;
        }
        #endregion
    }
}
