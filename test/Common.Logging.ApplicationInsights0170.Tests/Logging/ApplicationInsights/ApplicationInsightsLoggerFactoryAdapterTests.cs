#region License

/*
 * Copyright 2002-2015 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

namespace Common.Logging.ApplicationInsights.Tests
{
    using Common.Logging.ApplicationInsights;
    using Common.Logging.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// </summary>
    /// <author>Mihail Smacinih</author>
    [TestClass]
    public class ApplicationInsightsLoggerFactoryAdapterTests
    {
        [Owner("Mihail Smacinih")]
        [TestMethod]
        public void LogMessageAndException()
        {
            TestApplicationInsightsLoggerFactoryAdapter a = CreateTestApplicationInsightsLoggerFactoryAdapter();
            Exception ex = new Exception("errormessage");

            ILog log = a.GetLogger(this.GetType());
            log.Error("Message2", ex);
            Assert.AreEqual("Message2", a.LastLogEntry.Message);
            Assert.AreEqual(ex, a.LastLogEntry.Exception);
            Assert.AreEqual(LogLevel.Error, a.LastLogEntry.LogLevel);
        }

        [Owner("Mihail Smacinih")]
        [TestMethod]
        public void LogMessage()
        {
            TestApplicationInsightsLoggerFactoryAdapter a = CreateTestApplicationInsightsLoggerFactoryAdapter();

            ILog log = a.GetLogger(this.GetType());
            log.Info("Message2");
            Assert.AreEqual("Message2", a.LastLogEntry.Message);
            Assert.IsNull(a.LastLogEntry.Exception);
            Assert.AreEqual(LogLevel.Info, a.LastLogEntry.LogLevel);
        }

        [Owner("Mihail Smacinih")]
        [TestMethod]
        public void LogDebug()
        {
            TestApplicationInsightsLoggerFactoryAdapter a = CreateTestApplicationInsightsLoggerFactoryAdapter();
            ILog log = a.GetLogger(this.GetType());
            Exception ex = new Exception("errormessage3");
            log.Debug(null, ex);
            Assert.AreEqual(ex, a.LastLogEntry.Exception);
            Assert.AreEqual(LogLevel.Debug, a.LastLogEntry.LogLevel);
        }

        [Owner("Mihail Smacinih")]
        [TestMethod]
        public void LogException()
        {
            TestApplicationInsightsLoggerFactoryAdapter a = CreateTestApplicationInsightsLoggerFactoryAdapter();
            ILog log = a.GetLogger(this.GetType());
            Exception ex = new Exception("errormessage3");
            log.Error(null, ex);
            Assert.AreEqual(ex, a.LastLogEntry.Exception);
            Assert.AreEqual(LogLevel.Error, a.LastLogEntry.LogLevel);
        }

        private static TestApplicationInsightsLoggerFactoryAdapter CreateTestApplicationInsightsLoggerFactoryAdapter()
        {
            return new TestApplicationInsightsLoggerFactoryAdapter("7227BA1E-F6E4-4149-A776-94CC0F4D57C4");
        }

        private class TestApplicationInsightsLoggerFactoryAdapter: ApplicationInsightsLoggerFactoryAdapter
        {
            private readonly string instrumentationKey;
            
            public TestApplicationInsightsLoggerFactoryAdapter(string instrumentationKey) 
                : base(new NameValueCollection() { {"InstrumentationKey", instrumentationKey.ToString()} })
            {
                this.instrumentationKey = instrumentationKey;
            }

            public LogEntry LastLogEntry { get; set; }

            protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            {
                return new TestApplicationInsightsLogger(this, instrumentationKey, name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
            }

            public class TestApplicationInsightsLogger : ApplicationInsightsLogger
            {
                private TestApplicationInsightsLoggerFactoryAdapter owner;
                public TestApplicationInsightsLogger(TestApplicationInsightsLoggerFactoryAdapter owner, string instrumentationKey, string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat)
                    : base(instrumentationKey, logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
                {
                    this.owner = owner;
                }

                protected override void WriteInternal(LogLevel level, object message, Exception exception)
                {
                    this.owner.LastLogEntry = new LogEntry(level, message, exception);
                    base.WriteInternal(level, message, exception);
                }
            }
        }

        public class LogEntry
        {
            public LogEntry(LogLevel logLevel, object message, Exception exception)
            {
                this.LogLevel = logLevel;
                this.Message = message;
                this.Exception = exception;
            }

            public object Message { get; private set; }
            public Exception Exception { get; private set; }
            public LogLevel LogLevel { get; private set; }
        }
    }
}
