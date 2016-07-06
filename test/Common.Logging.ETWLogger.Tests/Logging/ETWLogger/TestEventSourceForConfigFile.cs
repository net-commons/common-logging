using System;
using Common.Logging.ETW;
using Microsoft.Diagnostics.Tracing;

namespace Common.Logging.ETWLogger.Tests
{
    public sealed class TestEventSourceForConfigFile : EventSource,ICommonLoggingEventSource
    {
        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void TraceException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        public void DebugException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        public void InfoException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        public void WarnException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        public void ErrorException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        public void FatalException(string message, string exception)
        {
            throw new NotImplementedException();
        }
    }
}