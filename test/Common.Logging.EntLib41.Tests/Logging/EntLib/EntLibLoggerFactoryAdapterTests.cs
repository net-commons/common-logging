using System;
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using NUnit.Framework;

namespace Common.Logging.EntLib
{
    [TestFixture]
    public class EntLibLoggerFactoryAdapterTests
    {
        [Test]
        public void InitDefaults()
        {
            EntLibLoggerFactoryAdapter a = new EntLibLoggerFactoryAdapter();
            Assert.AreEqual(EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT, a.ExceptionFormat);
            Assert.AreEqual(EntLibLoggerSettings.DEFAULTPRIORITY, a.DefaultPriority);

            a = new EntLibLoggerFactoryAdapter(null);
            Assert.AreEqual(EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT, a.ExceptionFormat);
            Assert.AreEqual(EntLibLoggerSettings.DEFAULTPRIORITY, a.DefaultPriority);
        }

        [Test]
        public void InitWithProperties()
        {
            NameValueCollection props = new NameValueCollection();
            props["exceptionFormat"] = "$(exception.message)";
            props["priority"] = "10";
             EntLibLoggerFactoryAdapter a = new EntLibLoggerFactoryAdapter(props);
            Assert.AreEqual("$(exception.message)", a.ExceptionFormat);
            Assert.AreEqual(10, a.DefaultPriority);
        }

        [Test]
        public void CachesLoggers()
        {
            SeverityFilter severityFilter = new SeverityFilter(null, TraceEventType.Critical | TraceEventType.Error);
            TestEntLibLoggerFactoryAdapter a = CreateTestEntLibLoggerFactoryAdapter(severityFilter);

            ILog log = a.GetLogger(this.GetType());
            Assert.AreSame( log, a.GetLogger(this.GetType()) );            
        }

        [Test]
        public void LogsMessage()
        {
            SeverityFilter severityFilter = new SeverityFilter(null, TraceEventType.Critical|TraceEventType.Error);            
            TestEntLibLoggerFactoryAdapter a = CreateTestEntLibLoggerFactoryAdapter(severityFilter);
            Exception ex = new Exception("errormessage");

            ILog log = a.GetLogger(this.GetType());

            // not logged due to severity filter 
            a.LastLogEntry = null;
            log.Trace("Message1", ex);
            Assert.IsNull(a.LastLogEntry);

            // logged, passes severity filter
            a.LastLogEntry = null;
            log.Error("Message2", ex);
            Assert.AreEqual( TraceEventType.Error, a.LastLogEntry.Severity );
            Assert.AreEqual( "Message2", a.LastLogEntry.Message);
            Assert.AreEqual( a.DefaultPriority, a.LastLogEntry.Priority);
            Assert.AreEqual( 1, a.LastLogEntry.Categories.Count);
            Assert.AreEqual( this.GetType().FullName, a.LastLogEntry.CategoriesStrings[0]);
            Assert.AreEqual("Exception[ message = errormessage, source = , targetsite = , stacktrace =  ]", a.LastLogEntry.ErrorMessages.Trim());
        }

        #region TestEntLibLoggerFactoryAdapter

        private static TestEntLibLoggerFactoryAdapter CreateTestEntLibLoggerFactoryAdapter(ILogFilter filter)
        {
            LogWriter logWriter = new LogWriter(
                new ILogFilter[] { filter }
                , new LogSource[] { new LogSource("logSource") }
                , new LogSource("defaultLogSource")
                , new LogSource("notProcessedLogSource")
                , new LogSource("errorsLogSource")
                , "DefaultCategory"
                , true
                , true
                );

            return new TestEntLibLoggerFactoryAdapter(5, EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT, logWriter);
        }

        private class TestEntLibLoggerFactoryAdapter: EntLibLoggerFactoryAdapter
        {
            public LogEntry LastLogEntry;

            public TestEntLibLoggerFactoryAdapter(int defaultPriority, string exceptionFormat, LogWriter logWriter) 
                : base(defaultPriority, exceptionFormat, logWriter)
            {
            }

            protected override ILog CreateLogger(string name, LogWriter logWriter, EntLibLoggerSettings settings)
            {
                return new TestEntLibLogger(this, name, logWriter, settings);
            }

            private class TestEntLibLogger : EntLibLogger
            {
                private readonly TestEntLibLoggerFactoryAdapter owner;

                public TestEntLibLogger(TestEntLibLoggerFactoryAdapter owner, string category, LogWriter logWriter, EntLibLoggerSettings settings) 
                    : base(category, logWriter, settings)
                {
                    this.owner = owner;
                }

                protected override void WriteLog(LogEntry log)
                {
                    owner.LastLogEntry = log;
                    base.WriteLog(log);
                }
            }
        }

        #endregion
    }
}
