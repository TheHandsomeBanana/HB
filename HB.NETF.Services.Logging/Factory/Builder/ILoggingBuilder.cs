using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory.Target;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Logging.Factory.Builder {
    public interface ILoggingBuilder {
        ILoggingBuilder AddTarget(object target);
        ILoggingBuilder AddTarget(Action<LogStatement> action);
        ILoggingBuilder AddTarget(Action<string> action);
        ILoggingBuilder AddTarget(string file);
        ILoggingBuilder AddTarget(Stream stream);

        ILoggingBuilder AddTargets(params object[] targets);
        ILoggingBuilder AddTargets(params Action<LogStatement>[] actions);
        ILoggingBuilder AddTargets(params Action<string>[] actions);
        ILoggingBuilder AddTargets(params string[] files);
        ILoggingBuilder AddTargets(params Stream[] files);

    }
}
