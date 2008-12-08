using System.Collections.Specialized;
using Common.TestUtil;
using NUnit.Framework;

namespace Common.Logging
{
    [TestFixture]
    public class CommonLoggingTraceListenerTests
    {
        [Test]
        public void DefaultSettings()
        {
            CommonLoggingTraceListener l = new CommonLoggingTraceListener();

            Assert.AreEqual("Diagnostics", l.Name);
            Assert.AreEqual(LogLevel.Trace, l.LogLevel);
        }

        [Test]
        public void ProcessesProperties()
        {
            CommonLoggingTraceListener l;

            NameValueCollection props = new NameValueCollection();
            props["Name"] = "TestName";
            props["LogLevel"] = LogLevel.Info.ToString().ToLower();
            l = new CommonLoggingTraceListener(props);

            Assert.AreEqual(props["name"], l.Name);
            Assert.AreEqual(LogLevel.Info, l.LogLevel);
        }

        [Test]
        public void ProcessesInitializeData()
        {
            CommonLoggingTraceListener l;

            // null results in default settings
            l = new CommonLoggingTraceListener((string)null);
            Assert.AreEqual("Diagnostics", l.Name);
            Assert.AreEqual(LogLevel.Trace, l.LogLevel);

            // string.Empty results in default settings
            l = new CommonLoggingTraceListener(string.Empty);
            Assert.AreEqual("Diagnostics", l.Name);
            Assert.AreEqual(LogLevel.Trace, l.LogLevel);

            // values are trimmed and case-insensitive
            l = new CommonLoggingTraceListener(" Name =  TestName\t;  LogLevel   =warn");
            Assert.AreEqual("TestName", l.Name);
            Assert.AreEqual(LogLevel.Warn, l.LogLevel);
        }

        [Test]
        public void LogsUsingCommonLogging()
        {
            ILoggerFactoryAdapter oldAdapter = LogManager.Adapter;

            try
            {
                TestLoggerFactoryAdapter factoryAdapter = new TestLoggerFactoryAdapter();
                LogManager.Adapter = factoryAdapter;

                CommonLoggingTraceListener l = new CommonLoggingTraceListener("Name=diagnostics;LogLevel=info");
                //TestLogger logger = (TestLogger) factoryAdapter.GetLogger(l.Name);

                l.Write((object)"TestMessage");
                Assert.AreEqual("diagnostics", factoryAdapter.LastEvent.Source.Name);
                Assert.AreEqual(LogLevel.Info, factoryAdapter.LastEvent.Level);
                Assert.AreEqual("TestMessage", factoryAdapter.LastEvent.RenderedMessage);
                Assert.AreEqual(null, factoryAdapter.LastEvent.Exception);

                l.LogLevel = LogLevel.Trace;
                l.Write("TestMessage1");
                Assert.AreEqual("diagnostics", factoryAdapter.LastEvent.Source.Name);
                Assert.AreEqual(LogLevel.Trace, factoryAdapter.LastEvent.Level);
                Assert.AreEqual("TestMessage1", factoryAdapter.LastEvent.RenderedMessage);
                Assert.AreEqual(null, factoryAdapter.LastEvent.Exception);

                l.LogLevel = LogLevel.Warn;
                l.WriteLine((object)"TestMessage2");
                Assert.AreEqual("diagnostics", factoryAdapter.LastEvent.Source.Name);
                Assert.AreEqual(LogLevel.Warn, factoryAdapter.LastEvent.Level);
                Assert.AreEqual("TestMessage2", factoryAdapter.LastEvent.RenderedMessage);
                Assert.AreEqual(null, factoryAdapter.LastEvent.Exception);

                // no logging at LogLevel.Off
                int eventCount = factoryAdapter.LoggerEvents.Count;
                l.LogLevel = LogLevel.Off;
                l.WriteLine("TestMessage3");
                Assert.AreEqual(eventCount, factoryAdapter.LoggerEvents.Count);
            }
            finally
            {
                LogManager.Adapter = oldAdapter;
            }
        }
    }
}
