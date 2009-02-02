using System.Collections.Specialized;
using System.Diagnostics;
using Common.TestUtil;
using NUnit.Framework;

namespace Common.Logging
{
    [TestFixture]
    public class CommonLoggingTraceListenerTests
    {
        [Test]
        public void LogsUsingCommonLogging()
        {
            ILoggerFactoryAdapter oldAdapter = LogManager.Adapter;

            try
            {
                TestLoggerFactoryAdapter factoryAdapter = new TestLoggerFactoryAdapter();
                LogManager.Adapter = factoryAdapter;

                CommonLoggingTraceListener l = new CommonLoggingTraceListener();                
                l.DefaultTraceEventType = (TraceEventType) 0xFFFF;

                AssertExpectedLogLevel(l, TraceEventType.Start, LogLevel.Trace);
                AssertExpectedLogLevel(l, TraceEventType.Stop, LogLevel.Trace);
                AssertExpectedLogLevel(l, TraceEventType.Suspend, LogLevel.Trace);
                AssertExpectedLogLevel(l, TraceEventType.Resume, LogLevel.Trace);
                AssertExpectedLogLevel(l, TraceEventType.Transfer, LogLevel.Trace);
                AssertExpectedLogLevel(l, TraceEventType.Verbose, LogLevel.Debug);
                AssertExpectedLogLevel(l, TraceEventType.Information, LogLevel.Info);
                AssertExpectedLogLevel(l, TraceEventType.Warning, LogLevel.Warn);
                AssertExpectedLogLevel(l, TraceEventType.Error, LogLevel.Error);
                AssertExpectedLogLevel(l, TraceEventType.Critical, LogLevel.Fatal);

                factoryAdapter.LastEvent = null;
                l.DefaultTraceEventType = TraceEventType.Warning;
                l.Write("some message", "some category");
                Assert.AreEqual(string.Format(l.LoggerNameFormat, l.Name, "some category"), factoryAdapter.LastEvent.Source.Name);
                Assert.AreEqual(LogLevel.Warn, factoryAdapter.LastEvent.Level);
                Assert.AreEqual("some message", factoryAdapter.LastEvent.RenderedMessage);
                Assert.AreEqual(null, factoryAdapter.LastEvent.Exception);
            }
            finally
            {
                LogManager.Adapter = oldAdapter;
            }
        }

        private void AssertExpectedLogLevel(CommonLoggingTraceListener l, TraceEventType eventType, LogLevel expectedLogLevel)
        {
            TestLoggerFactoryAdapter factoryAdapter = (TestLoggerFactoryAdapter) LogManager.Adapter;
            factoryAdapter.LastEvent = null;
            l.TraceEvent(null, "sourceName " + eventType, eventType, -1, "format {0}", eventType);
            Assert.AreEqual(string.Format(l.LoggerNameFormat, l.Name, "sourceName " + eventType), factoryAdapter.LastEvent.Source.Name);
            Assert.AreEqual(expectedLogLevel, factoryAdapter.LastEvent.Level);
            Assert.AreEqual("format " + eventType, factoryAdapter.LastEvent.RenderedMessage);
            Assert.AreEqual(null, factoryAdapter.LastEvent.Exception);
        }

        [Test]
        public void DoesNotLogBelowFilterLevel()
        {
            ILoggerFactoryAdapter oldAdapter = LogManager.Adapter;

            try
            {
                TestLoggerFactoryAdapter factoryAdapter = new TestLoggerFactoryAdapter();
                LogManager.Adapter = factoryAdapter;

                CommonLoggingTraceListener l = new CommonLoggingTraceListener();
                l.Filter = new EventTypeFilter(SourceLevels.Warning);
                factoryAdapter.LastEvent = null;
                l.TraceEvent(null, "sourceName", TraceEventType.Information, -1, "format {0}", "Information");
                Assert.AreEqual(null, factoryAdapter.LastEvent);

                AssertExpectedLogLevel(l, TraceEventType.Warning, LogLevel.Warn);
                AssertExpectedLogLevel(l, TraceEventType.Error, LogLevel.Error);
            }
            finally
            {
                LogManager.Adapter = oldAdapter;
            }            
        }

        [Test]
        public void DefaultSettings()
        {
            CommonLoggingTraceListener l = new CommonLoggingTraceListener();

            AssertDefaultSettings(l);
        }

        [Test]
        public void ProcessesProperties()
        {
            CommonLoggingTraceListener l;

            NameValueCollection props = new NameValueCollection();
            props["Name"] = "TestName";
            props["DefaultTraceEventType"] = TraceEventType.Information.ToString().ToLower();
            props["LoggerNameFormat"] = "{0}-{1}";
            l = new CommonLoggingTraceListener(props);

            Assert.AreEqual("TestName", l.Name);
            Assert.AreEqual(TraceEventType.Information, l.DefaultTraceEventType);
            Assert.AreEqual("{0}-{1}", l.LoggerNameFormat);
        }

        [Test]
        public void ProcessesInitializeData()
        {
            CommonLoggingTraceListener l;

            // null results in default settings
            l = new CommonLoggingTraceListener((string)null);
            AssertDefaultSettings(l);

            // string.Empty results in default settings
            l = new CommonLoggingTraceListener(string.Empty);
            AssertDefaultSettings(l);

            // values are trimmed and case-insensitive, empty values ignored
            l = new CommonLoggingTraceListener("; DefaultTraceeventtype   =warninG; loggernameFORMAT= {0}-{1}\t; Name =  TestName\t; ");
            Assert.AreEqual("TestName", l.Name);
            Assert.AreEqual(TraceEventType.Warning, l.DefaultTraceEventType);
            Assert.AreEqual("{0}-{1}", l.LoggerNameFormat);
        }

        private void AssertDefaultSettings(CommonLoggingTraceListener l)
        {
            Assert.AreEqual("Diagnostics", l.Name);
            Assert.AreEqual(TraceEventType.Verbose, l.DefaultTraceEventType);
            Assert.AreEqual("{0}.{1}", l.LoggerNameFormat);
        }
    }
}