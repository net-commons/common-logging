using System.Reflection;
using Common.Logging.NLog;
using NLog;
using NLog.Config;
using NUnit.Framework;

namespace Common.Logger.NLog
{
    [TestFixture]
    public class NLogLoggerFactoryAdapterTests
    {
        private class TestLoggingConfiguration : LoggingConfiguration
        {
            public readonly TestTarget Target;

            public TestLoggingConfiguration()
            {
                Target = new TestTarget();
                LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, Target));
            }
        }

        private class TestTarget : Target
        {
            public LogEventInfo LastLogEvent;
            protected override void Write(LogEventInfo logEvent)
            {
                LastLogEvent = logEvent;
            }

            protected override int NeedsStackTrace()
            {
                return 1;
            }
        }

        [Test]
        public void LogsUserStackFrame()
        {
            TestLoggingConfiguration cfg = new TestLoggingConfiguration();
            LogManager.Configuration = cfg;

            Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(null);
            Common.Logging.LogManager.GetLogger("myLogger").Debug("TestMessage");

            Assert.IsNotNull(cfg.Target.LastLogEvent);
            string stackTrace = cfg.Target.LastLogEvent.StackTrace.ToString();
            Assert.AreSame(MethodBase.GetCurrentMethod(), cfg.Target.LastLogEvent.UserStackFrame.GetMethod());
        }
    }
}
