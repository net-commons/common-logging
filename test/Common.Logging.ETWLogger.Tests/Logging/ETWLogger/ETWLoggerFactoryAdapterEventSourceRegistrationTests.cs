using System;
using Common.Logging.Configuration;
using Common.Logging.ETW;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class ETWLoggerFactoryAdapterEventSourceRegistrationTests
    {
        [Test]
        public void CanSetEventSourceToSameInstanceOfSameTypeMultipleTimes()
        {
            var eventSource = new TestEventSource4();
            var adapter = new ETWLoggerFactoryAdapter();

            //setting the event source to the same *instance* multiple times should not throw
            adapter.EventSource = eventSource;
            adapter.EventSource = eventSource;
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

        [Test]
        public void CanSetEventSourceToSameTypeMultipleTimesOnSingleAdapterWithPermitDuplicateSetToTrue()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
                    <arg key='permitDuplicateEventSourceRegistration' value='true'/>
                  </factoryAdapter>
                </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;

            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            adapter.EventSource = new TestEventSource5();
            adapter.EventSource = new TestEventSource5();
        }


        [Test]
        public void CanSetEventSourceToSameTypeMultipleTimesOnMultipleAdapterWithPermitDuplicateSetToTrue()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
                    <arg key='permitDuplicateEventSourceRegistration' value='true'/>
                  </factoryAdapter>
                </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;

            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");

            var adapter1 = new ETWLoggerFactoryAdapter(setting.Properties);
            var adapter2 = new ETWLoggerFactoryAdapter(setting.Properties);

            adapter1.EventSource = new TestEventSource5();
            adapter2.EventSource = new TestEventSource5();
        }
    }
}