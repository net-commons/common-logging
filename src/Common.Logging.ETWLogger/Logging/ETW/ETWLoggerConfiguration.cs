namespace Common.Logging.ETW
{
    public class ETWLoggerConfiguration
    {
        public bool IsTraceEnabled { get; set; }
        public bool IsDebugEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public bool IsInfoEnabled { get; set; }
        public bool IsWarnEnabled { get; set; }

        public ETWLoggerConfiguration()
        {
            IsTraceEnabled = true;
            IsDebugEnabled = true;
            IsErrorEnabled = true;
            IsFatalEnabled = true;
            IsInfoEnabled = true;
            IsWarnEnabled = true;
        }
    }
}