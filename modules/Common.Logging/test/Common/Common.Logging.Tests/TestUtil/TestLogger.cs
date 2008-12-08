using System;
using System.Collections;
using Common.Logging;
using Common.Logging.Simple;

namespace Common.TestUtil
{
    public class TestLogger : AbstractSimpleLogger
    {
        private readonly TestLoggerFactoryAdapter _owner;
        public TestLoggerEvent LastEvent;
 
        public TestLogger(TestLoggerFactoryAdapter owner, string logName) 
            : base(logName, LogLevel.All, true, true, null)
        {
            _owner = owner;
        }
           
        protected override void Write(LogLevel level, object message, Exception exception)
        {
            LastEvent = new TestLoggerEvent(this, level, message, exception);
            _owner.AddEvent(LastEvent);
        }
    }
}