#region License

/*
 * Copyright © 2002-2007 the original author or authors.
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

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Common.TestUtil;
using NUnit.Framework;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Exercises the TraceLogger implementation.
    /// </summary>
    /// <author>Mark Pollack</author>
    [TestFixture]
    public class TraceLoggerTests : AbstractSimpleLogTest
    {
        [SetUp]
        public void Setup()
        {
            NameValueCollection properties = GetProperties();

            // set Adapter
            LogManager.Adapter = new TraceLoggerFactoryAdapter(properties);
            defaultLogInstance = LogManager.GetLogger(LoggerType.FullName);
        }

        public override Type LoggerType
        {
            get { return typeof (TraceLogger); }
        }

        /// <summary>
        /// Basic checks specific to ConsoleOutLogger
        /// </summary>
        /// <param name="log">The log.</param>
        protected override void CheckLog(ILog log)
        {
            Assert.IsNotNull(log);
            Assert.IsInstanceOf<TraceLogger>(log);

            // Can we call level checkers with no exceptions?
            Assert.IsTrue(log.IsTraceEnabled);
            Assert.IsTrue(log.IsDebugEnabled);
            Assert.IsTrue(log.IsInfoEnabled);
            Assert.IsTrue(log.IsWarnEnabled);
            Assert.IsTrue(log.IsErrorEnabled);
            Assert.IsTrue(log.IsFatalEnabled);
        }

        [Test]
        public void UsesTraceSource()
        {
            Console.WriteLine("Config:"+ AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            Assert.AreEqual("FromAppConfig", ConfigurationManager.AppSettings["appConfigCheck"]);

            // just ensure, that <system.diagnostics> is configured for our test
            Trace.Refresh();
            TraceSource ts = new TraceSource("TraceLoggerTests", SourceLevels.All);
            Assert.AreEqual(1, ts.Listeners.Count);
            Assert.AreEqual(typeof(CapturingTraceListener), ts.Listeners[0].GetType());

            CapturingTraceListener.Events.Clear();
            ts.TraceEvent(TraceEventType.Information, 0, "message");
            Assert.AreEqual(TraceEventType.Information, CapturingTraceListener.Events[0].EventType);
            Assert.AreEqual("message", CapturingTraceListener.Events[0].FormattedMessage);

            // reset events and set loggerFactoryAdapter
            CapturingTraceListener.Events.Clear();
            NameValueCollection props = new NameValueCollection();
            props["useTraceSource"] = "TRUE";
            TraceLoggerFactoryAdapter adapter = new TraceLoggerFactoryAdapter(props);
            adapter.ShowDateTime = false;
            LogManager.Adapter = adapter;

            ILog log = LogManager.GetLogger("TraceLoggerTests");            
            log.WarnFormat("info {0}", "arg");
            Assert.AreEqual(TraceEventType.Warning, CapturingTraceListener.Events[0].EventType);
            Assert.AreEqual("[WARN]  TraceLoggerTests - info arg", CapturingTraceListener.Events[0].FormattedMessage);
        }
    }
}