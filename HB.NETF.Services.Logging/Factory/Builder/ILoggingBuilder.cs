using System;
using System.IO;

namespace HB.NETF.Services.Logging.Factory.Builder {
    public interface ILoggingBuilder {
        ILoggingBuilder AddTarget(object target, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(Action<LogStatement> action, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(Action<string> action, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(string file, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(Stream stream, LogSeverity minSeverity = LogSeverity.Trace);

        ILoggingBuilder AddTargets(object[] targets, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTargets(Action<LogStatement>[] actions, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTargets(Action<string>[] actions, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTargets(string[] files, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTargets(Stream[] files, LogSeverity minSeverity = LogSeverity.Trace);

    }
}
