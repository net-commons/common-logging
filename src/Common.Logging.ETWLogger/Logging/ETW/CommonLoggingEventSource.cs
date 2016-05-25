using System;
using System.Diagnostics.Eventing.Reader;
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

        [Event(11)]
        public void TraceException(string message, string exception)
        {
            WriteEvent(11, message, exception);
        }

        [Event(12)]
        public void DebugException(string message, string exception)
        {
            WriteEvent(12, message, exception);
        }

        [Event(13)]
        public void InfoException(string message, string exception)
        {
            WriteEvent(13, message, exception);
        }

        [Event(14)]
        public void WarnException(string message, string exception)
        {
            WriteEvent(14, message, exception);
        }

        [Event(15)]
        public void ErrorException(string message, string exception)
        {
            WriteEvent(15, message, exception);
        }

        [Event(16)]
        public void FatalException(string message, string exception)
        {
            WriteEvent(16, message, exception);
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

        void TraceException(string message, string exception);
        void DebugException(string message, string exception);
        void InfoException(string message, string exception);
        void WarnException(string message, string exception);
        void ErrorException(string message, string exception);
        void FatalException(string message, string exception);
    }
}