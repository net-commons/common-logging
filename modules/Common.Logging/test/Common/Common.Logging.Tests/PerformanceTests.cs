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

#if NET_3_0

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using Common.Logging;
using Common.Logging.Simple;
using NUnit.Framework;

namespace Common
{
    public class MyClass
    {
        private readonly TraceSource trace = new TraceSource(typeof(MyClass).Name);

        private void Foo(int id)
        {
            try
            {
                Bar(id);
            }
            catch (Exception ex)
            {
                trace.TraceEvent(TraceEventType.Information, -1, "error id={0}:{1}", id, ex);
            }
        }

        private void Bar(int id) { }
    }

    internal class MyTestObjectUnderTrace
    {
        public override string ToString()
        {
            // a typical ToString() implementation of user-defined types
            return string.Format("{0}#{1} {{2}}", this.GetType().Name, this.GetHashCode(), "some info");
        }
    }

    public class MyTestTraceListener : TraceListener
    {
        public readonly ArrayList Messages = new ArrayList();

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
            {
                // emulates the underlying call to TraceEvent/WriteLine
                string message = string.Format(format, args);
                Messages.Add(message);
            }
            //            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        public override void Write(string message)
        {
            // ensure we don't get called accidentially
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            // ensure we don't get called accidentially
            throw new NotImplementedException();
        }
    }

    public class MyTestLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        private static readonly ArrayList s_messages = new ArrayList();

        public ArrayList Messages = s_messages;

        private class MySimpleLogger : AbstractSimpleLogger
        {
            private readonly IList messages;

            public MySimpleLogger(ArrayList messages, string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
                : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
            {
                this.messages = messages;
            }

            protected override void WriteInternal(LogLevel level, object message, Exception exception)
            {
                messages.Add(message);
            }
        }

        public MyTestLoggerFactoryAdapter(NameValueCollection properties)
            : base(properties)
        {
        }

        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            return new MySimpleLogger(Messages, name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
    }

    [TestFixture, Explicit]
    public class PerformanceTests
    {
        [Test]
        public void JustForTestingLoggingConfiguration()
        {
            TraceSource traceSource = new TraceSource("DiagnosticsTracePerformanceTest");
            traceSource.TraceEvent(TraceEventType.Verbose, -1, "test trace info {0}", new MyTestObjectUnderTrace());
            //            Trace.TraceInformation("traced info");
            Trace.WriteLine("some message", "myCategory");
            Console.WriteLine("from console");
        }

        [Test]
        public void DiagnosticsTracePerformanceTest()
        {
            MyTestObjectUnderTrace myObj = new MyTestObjectUnderTrace();
            int runs = 100000000;
            StopWatch sw;
            MyTestTraceListener listener;

            // use NoOp logging
            LogManager.Adapter = new NoOpLoggerFactoryAdapter();
            ILog log = LogManager.GetLogger(this.GetType());

            sw = new StopWatch();
            using (sw.Start("Time:{0} - log.InfoFormat + NoOpLogger"))
            {
                for (int i = 0; i < runs; i++)
                {
                    log.Info( m=>m("some logger info {0}", (object)myObj) );
                }
            }

            // Use unconfigured TraceSource
            TraceSource traceSource = new TraceSource("bla");
            sw = new StopWatch();
            using (sw.Start("Time:{0} - traceSource.TraceEvent + unconfigured TraceSource"))
            {
                for (int i = 0; i < runs; i++)
                {
                    if (traceSource.Switch.ShouldTrace(TraceEventType.Information))
                    {
                        traceSource.TraceEvent(TraceEventType.Information, -1, "some tracesource info {0}", (object)myObj);
                    }
                }
            }

            // use Common.Logging
            MyTestLoggerFactoryAdapter adapter = new MyTestLoggerFactoryAdapter(null);
            adapter.ShowLogName = true;
            adapter.ShowDateTime = true;
            adapter.Level = LogLevel.Warn;
            LogManager.Adapter = adapter;
            log = LogManager.GetLogger(this.GetType());

            sw = new StopWatch();
            using (sw.Start("Time:{0} - log.InfoFormat"))
            {
                for (int i = 0; i < runs; i++)
                {
                    log.Info(m => m("some logger info {0}", (object)myObj));
                }
            }
            Assert.AreEqual(0, adapter.Messages.Count);

            // Use configured TraceSource
            traceSource = new TraceSource("DiagnosticsTracePerformanceTest");
            listener = (MyTestTraceListener) traceSource.Listeners[0];
            sw = new StopWatch();
            using (sw.Start("Time:{0} - traceSource.TraceEvent"))
            {
                for (int i = 0; i < runs; i++)
                {
                    if (traceSource.Switch.ShouldTrace(TraceEventType.Information))
                    {
                        traceSource.TraceEvent(TraceEventType.Information, -1, "some tracesource info {0}", (object)myObj);
                    }
                }
            }
            Assert.AreEqual(0, listener.Messages.Count);
        }
    }
}

#endif