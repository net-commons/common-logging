using System;
using Common.Logging;

namespace Common.TestUtil
{
    public class TestLoggerEvent
    {
        public readonly TestLogger Source;
        public readonly LogLevel Level;
        public readonly object Message;
        public readonly Exception Exception;

        public string RenderedMessage
        {
            get { return Message.ToString(); }
        }

        public TestLoggerEvent(TestLogger source, LogLevel level, object message, Exception exception)
        {
            Source = source;
            Level = level;
            Message = message;
            Exception = exception;
        }
    }
}