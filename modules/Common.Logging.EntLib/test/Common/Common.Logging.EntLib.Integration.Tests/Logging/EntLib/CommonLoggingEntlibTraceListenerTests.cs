#region License

/*
 * Copyright 2002-2009 the original author or authors.
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

using System.Diagnostics;
using Common.Logging.Simple;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using NUnit.Framework;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// </summary>
    /// <author>Erich Eichinger</author>
    [TestFixture]
    public class CommonLoggingEntlibTraceListenerTests
    {
        [Test]
        public void RedirectsToCommonLogging()
        {
            CapturingLoggerFactoryAdapter testLoggerFactoryAdapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = testLoggerFactoryAdapter;

            // force entlib logging init
            Logger.Write("init");

            // ensure external configuration didn't change
            Assert.AreEqual("Test Capturing Listener", TestCommonLoggingEntlibTraceListener.Instance.Name);
            LogEntry logEntry = new LogEntry();
            logEntry.Categories.Add("mycategory");
            logEntry.Message = "testmessage";
            ILogFormatter formatter = TestCommonLoggingEntlibTraceListener.Instance.Formatter;
            string s = formatter.Format(logEntry);
            Assert.AreEqual("Category: mycategory, Message: testmessage", s);

            using (TestCommonLoggingEntlibTraceListener.Instance.Capture())
            {
                // note that output depends on the formatter configured for the entlib listener!
                Logger.Write("message1");
                Assert.AreEqual("Category: General, Message: message1", testLoggerFactoryAdapter.LastEvent.RenderedMessage);
                Assert.AreEqual(LogLevel.Info, testLoggerFactoryAdapter.LastEvent.Level);

                Logger.Write("message2", "custom category", -1, -1, TraceEventType.Warning);
                Assert.AreEqual("Category: custom category, Message: message2", testLoggerFactoryAdapter.LastEvent.RenderedMessage);
                Assert.AreEqual("Test Capturing Listener/All Events", testLoggerFactoryAdapter.LastEvent.Source.Name);
                Assert.AreEqual(LogLevel.Warn, testLoggerFactoryAdapter.LastEvent.Level);
            }
        }
    }
}