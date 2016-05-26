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
        public override bool IsTraceEnabled { get { return Configuration.IsTraceEnabled; } }
        public override bool IsDebugEnabled { get { return Configuration.IsDebugEnabled; } }
        public override bool IsErrorEnabled { get { return Configuration.IsErrorEnabled; } }
        public override bool IsFatalEnabled { get { return Configuration.IsFatalEnabled; } }
        public override bool IsInfoEnabled { get { return Configuration.IsInfoEnabled; } }
        public override bool IsWarnEnabled { get { return Configuration.IsWarnEnabled; } }

        public ETWLoggerConfiguration Configuration { private get; set; }

        private readonly ICommonLoggingEventSource _eventSource;

        public ETWLogger(ICommonLoggingEventSource eventSource)
            : this(eventSource, new ETWLoggerConfiguration())
        { }

        public ETWLogger(ICommonLoggingEventSource eventSource, ETWLoggerConfiguration configuration)
        {
            _eventSource = eventSource;
            Configuration = configuration;
        }


        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {

            switch (level)
            {
                case LogLevel.All:
                    InvokeMethodOnEventSource(message, exception, _eventSource.Trace, _eventSource.TraceException);
                    break;
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
                    throw new ArgumentOutOfRangeException(nameof(level), level, "invalid logging level");
            }
        }

        private void InvokeMethodOnEventSource(object message, Exception exception, Action<string> noExceptionMethod, Action<string, string> exceptionMethod)
        {
            if (null == exception)
            {
                noExceptionMethod.Invoke(message.ToString());
            }
            else
            {
                exceptionMethod.Invoke(message.ToString(), exception.ToString());
            }
        }
    }
}
