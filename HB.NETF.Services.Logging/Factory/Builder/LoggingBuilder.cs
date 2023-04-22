using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Logging.Factory.Builder {
    internal class LoggingBuilder : ILoggingBuilder {
        private List<LogTarget> logTargets;
        public IReadOnlyList<LogTarget> LogTargets { get => logTargets; }

        public LoggingBuilder() {
            logTargets = new List<LogTarget>();
        }

        #region Target Handler
        public ILoggingBuilder WithNoTargets() => this;

        public ILoggingBuilder AddTarget(object target) {
            LogTarget logTarget = new LogTarget(target);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Action<LogStatement> action) {
            LogTarget logTarget = new LogTarget(action);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Action<string> action) {
            LogTarget logTarget = new LogTarget(action);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(string file) {
            LogTarget logTarget = new LogTarget(file);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTarget(Stream stream) {
            LogTarget logTarget = new LogTarget(stream);
            logTargets.Add(logTarget);
            return this;
        }

        public ILoggingBuilder AddTargets(params object[] targets) {
            foreach (object target in targets)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(params Action<LogStatement>[] actions) {
            foreach (Action<LogStatement> target in actions)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(params Action<string>[] actions) {
            foreach (Action<string> target in actions)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(params string[] files) {
            foreach (string target in files)
                AddTarget(target);

            return this;
        }

        public ILoggingBuilder AddTargets(params Stream[] streams) {
            foreach (Stream target in streams)
                AddTarget(target);

            return this;
        }
        #endregion
    }
}
