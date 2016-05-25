using Microsoft.Diagnostics.Tracing;

namespace Common.Logging.ETW
{
    [EventSource(Name = "Common.Logging.ETWLogger")]
    public sealed class CommonLoggingEventSource : EventSource, ICommonLoggingEventSource
    {
        [Event(1)]
        public void Trace(string message)
        {
            WriteEvent(1, message);
        }

        [Event(2)]
        public void Debug(string message)
        {
            WriteEvent(2, message);
        }

        [Event(3)]
        public void Info(string message)
        {
            WriteEvent(3, message);
        }

        [Event(4)]
        public void Warn(string message)
        {
            WriteEvent(4, message);
        }

        [Event(5)]
        public void Error(string message)
        {
            WriteEvent(5, message);
        }

        [Event(6)]
        public void Fatal(string message)
        {
            WriteEvent(6, message);
        }
    }

    public interface ICommonLoggingEventSource
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
    }
}