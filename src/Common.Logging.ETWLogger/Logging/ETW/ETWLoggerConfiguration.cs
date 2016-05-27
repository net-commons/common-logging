namespace Common.Logging.ETW
{
    public class ETWLoggerConfiguration
    {
        public bool IsTraceEnabled { get { return LogLevel.HasFlag(LogLevel.Trace); } }
        public bool IsDebugEnabled { get { return LogLevel.HasFlag(LogLevel.Debug); } }
        public bool IsErrorEnabled { get { return LogLevel.HasFlag(LogLevel.Error); } }
        public bool IsFatalEnabled { get { return LogLevel.HasFlag(LogLevel.Fatal); } }
        public bool IsInfoEnabled { get { return LogLevel.HasFlag(LogLevel.Info); } }
        public bool IsWarnEnabled { get { return LogLevel.HasFlag(LogLevel.Warn); } }


        public LogLevel LogLevel { get; set; }

    }
}