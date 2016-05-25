using System;
using System.Collections.Generic;
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
        //TODO: think carefully about how to deal with toggling these values when there's only GETTERs to override ...
        public override bool IsTraceEnabled { get; }
        public override bool IsDebugEnabled { get; }
        public override bool IsErrorEnabled { get; }
        public override bool IsFatalEnabled { get; }
        public override bool IsInfoEnabled { get; }
        public override bool IsWarnEnabled { get { return true; } }

        private readonly ICommonLoggingEventSource _eventSource;

        public ETWLogger(ICommonLoggingEventSource eventSource)
        {
            _eventSource = eventSource;
        }


        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.All:
                    _eventSource.Trace(message.ToString());
                    break;
                case LogLevel.Trace:
                    _eventSource.Trace(message.ToString());
                    break;
                case LogLevel.Debug:
                    _eventSource.Debug(message.ToString());
                    break;
                case LogLevel.Info:
                    _eventSource.Info(message.ToString());
                    break;
                case LogLevel.Warn:
                    _eventSource.Warn(message.ToString());
                    break;
                case LogLevel.Error:
                    _eventSource.Error(message.ToString());
                    break;
                case LogLevel.Fatal:
                    _eventSource.Fatal(message.ToString());
                    break;
                case LogLevel.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "invalid logging level");
            }
        }
    }
}
