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
            var logger = adapter.GetLogger(string.Empty);

            logger.Warn("This is a test message from ETW source!");
        }

        [Test]
        public void LoggingWithCustomEventSource()
        {
            var adapter = new ETWLoggerFactoryAdapter { ETWEventSource = new MyTestLoggerEventSource() };
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
        public void LoggingLevelImplicitlyIncludesGreaterLoggingLevels()
        {
            Assume.That(LogLevel.Fatal > LogLevel.Debug, "Relationship between FATAL and DEBUG logging levels not as expected.");

            var props = new NameValueCollection { { "level", "debug" } };
            var adapter = new ETWLoggerFactoryAdapter(props);

            var logger = adapter.GetLogger(string.Empty);

            Assert.That(logger.IsFatalEnabled);
        }


        [Test]
        public void CanUseDefaultLogLevelIfNoneSpecifiedInConfigFile()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;


            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            Assert.That(adapter.LogLevel.HasFlag(LogLevel.All));

        }

        [Test]
        public void CanReadLogLevelFromConfigFile()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
        arg key='level' value='warn' />
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;


            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            Assert.That(adapter.LogLevel.HasFlag(LogLevel.Warn));

        }

        [Test]
        public void CanUseDefaultEventSourceTypeIfNoneSpecifiedInConfigFile()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;


            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            Assert.That(adapter.ETWEventSource, Is.TypeOf<CommonLoggingEventSource>());

        }

        [Test]
        public void CanReadCustomCommonLoggingEventSourceTypeFromConfigFile()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
        <arg key='commonLoggingEventSourceType' value='Common.Logging.ETWLogger.Tests.MyTestLoggerEventSource,Common.Logging.ETWLogger.Tests'/>
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;


            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            Assert.That(adapter.ETWEventSource, Is.TypeOf<MyTestLoggerEventSource>());

        }


        [Test]
        public void ThrowsOnUnableToDetermineCustomCommonLoggingEventSourceType()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
        <arg key='commonLoggingEventSourceType' value='I am not a type specification.'/>
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;

            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");
            Assert.Throws<ConfigurationException>(() => new ETWLoggerFactoryAdapter(setting.Properties));
        }


        [Test]
        public void ThrowsOnCustomCommonLoggingEventSourceTypeIsWrongType()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
        <arg key='commonLoggingEventSourceType' value='Common.Logging.ETW.ETWLogger,Common.Logging.ETWLogger'/>
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;

            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");
            Assert.Throws<ConfigurationException>(() => new ETWLoggerFactoryAdapter(setting.Properties));
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
