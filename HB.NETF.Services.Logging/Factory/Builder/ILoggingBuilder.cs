using System;
using System.IO;

namespace HB.NETF.Services.Logging.Factory.Builder {
    public interface ILoggingBuilder {
        ILoggingBuilder AddTarget(object target, params LogSeverity[] severities);
        ILoggingBuilder AddTarget(Action<LogStatement> action, params LogSeverity[] severities);
        ILoggingBuilder AddTarget(Action<string> action, params LogSeverity[] severities);
        ILoggingBuilder AddTarget(string file, params LogSeverity[] severities);
        ILoggingBuilder AddTarget(Stream stream, params LogSeverity[] severities);

        ILoggingBuilder AddTargets(object[] targets, params LogSeverity[] severities);
        ILoggingBuilder AddTargets(Action<LogStatement>[] actions, params LogSeverity[] severities);
        ILoggingBuilder AddTargets(Action<string>[] actions, params LogSeverity[] severities);
        ILoggingBuilder AddTargets(string[] files, params LogSeverity[] severities);
        ILoggingBuilder AddTargets(Stream[] files, params LogSeverity[] severities);

    }
}
