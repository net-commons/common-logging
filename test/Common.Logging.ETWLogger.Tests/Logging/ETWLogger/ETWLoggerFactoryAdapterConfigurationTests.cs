using Common.Logging.Configuration;
using Common.Logging.ETW;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class ETWLoggerFactoryAdapterConfigurationTests
    {
        [Test]
        public void CanUseDefaultLogLevelIfNoneSpecifiedInConfigFile()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
                  </factoryAdapter>
                </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;


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
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;


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
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;


            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            Assert.That(adapter.EventSource, Is.TypeOf<CommonLoggingEventSource>());

        }

        [Test]
        public void CanReadCustomCommonLoggingEventSourceTypeFromConfigFile()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <factoryAdapter type='Common.Logging.ETW.ETWLoggerFactoryAdapter, Common.Logging.ETWLogger'>
                    <arg key='commonLoggingEventSourceType' value='Common.Logging.ETWLogger.Tests.TestEventSourceForConfigFile,Common.Logging.ETWLogger.Tests'/>
                  </factoryAdapter>
                </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;


            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");


            var adapter = new ETWLoggerFactoryAdapter(setting.Properties);

            Assert.That(adapter.EventSource, Is.TypeOf<TestEventSourceForConfigFile>());

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
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;

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
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;

            Assume.That(setting, Is.Not.Null, "Failed to parse config to create expected LogSetting instance.");
            Assert.Throws<ConfigurationException>(() => new ETWLoggerFactoryAdapter(setting.Properties));
        }
    }
}