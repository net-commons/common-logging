using System;
using Common.Logging.ETW;
using Microsoft.Diagnostics.Tracing;

namespace Common.Logging.ETWLogger.Tests
{
    [EventSource(Name = "Common.Logging.CustomTestEventSource")]
    public sealed class CustomTestEventSource : EventSource, ICommonLoggingEventSource
    {
        [Event(1)]
        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        [Event(2)]
        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        [Event(3)]
        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        [Event(4)]
        public void Warn(string message)
        {
            //note: method intentionally ignores the 'message' arg passed in order to demonstrate different behavior in the test
            // that can be seen when reviewing ETW output via e.g., PerfView or other tools
            WriteEvent(4, "This is a message from a custom logging source class.");
        }

        [Event(5)]
        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        [Event(6)]
        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        [Event(11)]
        public void TraceException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(12)]
        public void DebugException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(13)]
        public void InfoException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(14)]
        public void WarnException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(15)]
        public void ErrorException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(16)]
        public void FatalException(string message, string exception)
        {
            throw new NotImplementedException();
        }

    }
}