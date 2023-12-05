using System;
using System.IO;

namespace HB.NETF.Services.Logging.Factory.Builder {
    public interface ILoggingBuilder {
        ILoggingBuilder AddTarget(object target, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(Action<LogStatement> action, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(Action<string> action, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(string file, LogSeverity minSeverity = LogSeverity.Trace);
        ILoggingBuilder AddTarget(Stream stream, LogSeverity minSeverity = LogSeverity.Trace);

        ILoggingBuilder AddTargets(LogSeverity minSeverity = LogSeverity.Trace, params object[] targets);
        ILoggingBuilder AddTargets(LogSeverity minSeverity = LogSeverity.Trace, params Action<LogStatement>[] actions);
        ILoggingBuilder AddTargets(LogSeverity minSeverity = LogSeverity.Trace, params Action<string>[] actions);
        ILoggingBuilder AddTargets(LogSeverity minSeverity = LogSeverity.Trace, params string[] files);
        ILoggingBuilder AddTargets(LogSeverity minSeverity = LogSeverity.Trace, params Stream[] files);

    }
}
