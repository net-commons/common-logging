using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            logger.Warn("This is a test message from ETW source!");
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
            WriteEvent(4,"This is a message from a custom logging source class.");
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
    }
}
