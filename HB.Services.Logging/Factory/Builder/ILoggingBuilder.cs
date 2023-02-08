using HB.Services.Logging;
using HB.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Logging.Factory.Builder {
    public interface ILoggingBuilder {
        public ILoggingBuilder AddTarget(object target);
        public ILoggingBuilder AddTarget(Action<LogStatement> action);
        public ILoggingBuilder AddTarget(Action<string> action);
        public ILoggingBuilder AddTarget(string file);
        public ILoggingBuilder AddTarget(Stream stream);

        public ILoggingBuilder AddTargets(params object[] targets);
        public ILoggingBuilder AddTargets(params Action<LogStatement>[] actions);
        public ILoggingBuilder AddTargets(params Action<string>[] actions);
        public ILoggingBuilder AddTargets(params string[] files);
        public ILoggingBuilder AddTargets(params Stream[] files);

    }
}
