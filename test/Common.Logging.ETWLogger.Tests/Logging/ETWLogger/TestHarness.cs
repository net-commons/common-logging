using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging.Configuration;
using Common.Logging.ETW;
using Microsoft.Diagnostics.Tracing;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class TestHarness
    {
        [Test]
        public void BasicLoggingScenario()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(typeof(TestHarness));

            logger.Warn("This is a test message from ETW source!");
        }

        [Test]
        public void LoggingWithCustomEventSource()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            adapter.ETWEventSource = new MyTestLoggerEventSource();
            var logger = adapter.GetLogger(typeof(TestHarness));

            logger.Warn("This message should never appear in the ETW logs b/c its ignored by the custom logger!");
        }

        [Test]
        public void LoggingWithException()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(typeof(TestHarness));

            logger.Debug("This is a test message from ETW source!");
        }


        [Test]
        public void MyMethod()
        {
            var props = new NameValueCollection { { "level", "debug" } };
            var adapter = new ETWLoggerFactoryAdapter(props);

            Assert.That(adapter.Configuration.IsTraceEnabled);
        }
    }



    public sealed class MyTestLoggerEventSource : EventSource, ICommonLoggingEventSource
    {
        [Event(1)]
        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        [Event(2)]
        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        [Event(3)]
        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        [Event(4)]
        public void Warn(string message)
        {
            //note: method intentionally ignores the 'message' arg passed in order to demonstrate different behavior in the test
            WriteEvent(4, "This is a message from a custom logging source class.");
        }

        [Event(5)]
        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        [Event(6)]
        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        [Event(11)]
        public void TraceException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(12)]
        public void DebugException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(13)]
        public void InfoException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(14)]
        public void WarnException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(15)]
        public void ErrorException(string message, string exception)
        {
            throw new NotImplementedException();
        }

        [Event(16)]
        public void FatalException(string message, string exception)
        {
            throw new NotImplementedException();
        }

    }
}
