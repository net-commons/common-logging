using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Common.Logging.Configuration;
using Common.Logging.ETW;
using NUnit.Framework;
using Rhino.Mocks;

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
            var adapter = new ETWLoggerFactoryAdapter { EventSource = new MyCustomEventSource() };
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

        [Test]
        public void ByDefaultCannotSetEventSourceToSameTypeMultipleTimesOnSingleAdapter()
        {
            //setting the EventSource once is permitted
            var adapter = new ETWLoggerFactoryAdapter { EventSource = new TestEventSource3() };

            //setting it to another instance of the same type again is not
            Assert.Throws<InvalidOperationException>(() => adapter.EventSource = new TestEventSource3());
        }

        
        [Test]
        public void ByDefaultCannotSetEventSourceToSameTypeMultipleTimesEvenOnDifferentAdapters()
        {
            //register the event source with an adapter once
            new ETWLoggerFactoryAdapter { EventSource = new DuplicateRegisteredEventSource() };

            //registering the same event source (type) a second time should throw even on a different adapter
            Assert.Throws<InvalidOperationException>(() => new ETWLoggerFactoryAdapter { EventSource = new DuplicateRegisteredEventSource() });
        }

        [Test]
        public void CanSetDifferentEventSourceTypesOnMultipleAdapters()
        {
            var adapter1 = new ETWLoggerFactoryAdapter { EventSource = new TestEventSource1() };
            var adapter2 = new ETWLoggerFactoryAdapter { EventSource = new TestEventSource2() };

            Assert.That(adapter1.EventSource is TestEventSource1);
            Assert.That(adapter2.EventSource is TestEventSource2);
        }


    }
}
