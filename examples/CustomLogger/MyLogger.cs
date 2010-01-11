using System;
using Common.Logging;
using Common.Logging.Factory;

namespace CustomLogger
{
    public class MyLogger : AbstractLogger
    {
        private string _loggerName;
        private LogLevel _useLogLevel;

        public MyLogger(string loggerName, LogLevel useLogLevel)
        {
            _loggerName = loggerName;
            _useLogLevel = useLogLevel;
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            // write to whatever logsystem
            if (exception != null)
            {
                Console.WriteLine("Logging from {0}: {1} {2}", _loggerName, message, exception);                
            }
            else
            {
                Console.WriteLine("Logging from {0}: {1}", _loggerName, message);                
            }
        }

        public override bool IsTraceEnabled
        {
            get { return (_useLogLevel <= LogLevel.Trace ) ; }
        }

        public override bool IsDebugEnabled
        {
            get { return (_useLogLevel <= LogLevel.Debug); }
        }

        public override bool IsInfoEnabled
        {
            get { return (_useLogLevel <= LogLevel.Info); }
        }

        public override bool IsWarnEnabled
        {
            get { return (_useLogLevel <= LogLevel.Warn); }
        }

        public override bool IsErrorEnabled
        {
            get { return (_useLogLevel <= LogLevel.Error); }
        }

        public override bool IsFatalEnabled
        {
            get { return (_useLogLevel <= LogLevel.Fatal); }
        }
    }
}