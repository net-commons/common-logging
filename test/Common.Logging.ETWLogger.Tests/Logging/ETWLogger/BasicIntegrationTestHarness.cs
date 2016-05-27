using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging.Configuration;
using Common.Logging.ETW;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class BasicIntegrationTestHarness
    {
        [Test]
        public void BasicLoggingScenario()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(string.Empty);

            logger.Warn("This is a test message from ETW source!");
        }

        [Test]
        public void LoggingWithCustomEventSource()
        {
            var adapter = new ETWLoggerFactoryAdapter { ETWEventSource = new MyCustomEventSource() };
            var logger = adapter.GetLogger(string.Empty);

            logger.Warn("This message should never appear in the ETW logs b/c its ignored by the custom EventSource for the sake of the test!");
        }

        [Test]
        public void LoggingWithException()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(string.Empty);

            logger.Debug("This is a test message from ETW source!", new Exception("I am the test exception"));
        }
    }
}
