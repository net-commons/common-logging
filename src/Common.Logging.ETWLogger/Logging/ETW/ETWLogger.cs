using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing;
using Common.Logging;
using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public sealed class ETWLogger : AbstractLogger
    {
        public override bool IsTraceEnabled { get { return _logLevel.HasFlag(LogLevel.Trace); } }
        public override bool IsDebugEnabled { get { return _logLevel.HasFlag(LogLevel.Debug); } }
        public override bool IsErrorEnabled { get { return _logLevel.HasFlag(LogLevel.Error); } }
        public override bool IsFatalEnabled { get { return _logLevel.HasFlag(LogLevel.Fatal); } }
        public override bool IsInfoEnabled { get { return _logLevel.HasFlag(LogLevel.Info); } }
        public override bool IsWarnEnabled { get { return _logLevel.HasFlag(LogLevel.Warn); } }


        private readonly ICommonLoggingEventSource _eventSource;
        private LogLevel _logLevel;

        public ETWLogger(ICommonLoggingEventSource eventSource)
            : this(eventSource, LogLevel.All)
        { }

        public ETWLogger(ICommonLoggingEventSource eventSource, LogLevel logLevel)
        {
            _eventSource = eventSource;
            _logLevel = logLevel;
        }


        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {

            switch (level)
            {
                case LogLevel.All:
                case LogLevel.Trace:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Trace, _eventSource.TraceException);
                    break;

                case LogLevel.Debug:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Debug, _eventSource.DebugException);
                    break;

                case LogLevel.Info:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Info, _eventSource.InfoException);
                    break;

                case LogLevel.Warn:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Warn, _eventSource.WarnException);
                    break;

                case LogLevel.Error:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Error, _eventSource.ErrorException);
                    break;

                case LogLevel.Fatal:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Fatal, _eventSource.FatalException);
                    break;

                case LogLevel.Off:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("level", level, "invalid logging level");
            }
        }

        private void InvokeMethodOnEventSource(object message, Exception exception, Action<string> noExceptionMethod, Action<string, string> exceptionMethod)
        {
            if (null == exception && null != message)
            {
                noExceptionMethod.Invoke(message.ToString());
                return;
            }

            if (null == exception)
            {
                noExceptionMethod.Invoke(null);
                return;
            }

            exceptionMethod.Invoke(message.ToString(), exception.ToString());

        }
    }
}
