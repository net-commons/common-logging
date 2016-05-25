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
        public override bool IsTraceEnabled { get; }
        public override bool IsDebugEnabled { get; }
        public override bool IsErrorEnabled { get; }
        public override bool IsFatalEnabled { get; }
        public override bool IsInfoEnabled { get; }
        public override bool IsWarnEnabled { get { return true; } }


        public ICommonLoggingEventSource ETWEventSource { get; set; }

        public ETWLogger()
        {
            ETWEventSource = new CommonLoggingEventSource();
        }


        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.All:
                    ETWEventSource.Trace(message.ToString());
                    break;
                case LogLevel.Trace:
                    ETWEventSource.Trace(message.ToString());
                    break;
                case LogLevel.Debug:
                    ETWEventSource.Debug(message.ToString());
                    break;
                case LogLevel.Info:
                    ETWEventSource.Info(message.ToString());
                    break;
                case LogLevel.Warn:
                    ETWEventSource.Warn(message.ToString());
                    break;
                case LogLevel.Error:
                    ETWEventSource.Error(message.ToString());
                    break;
                case LogLevel.Fatal:
                    ETWEventSource.Fatal(message.ToString());
                    break;
                case LogLevel.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "invalid logging level");
            }
        }
    }
}
