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

using System.Collections.Specialized;
using System.Diagnostics;
using Common.Logging.Configuration;
using Common.Logging.Simple;
using NUnit.Framework;
using Rhino.Mocks;

namespace Common.Logging
{
    /// <summary>
    /// Tests for LogManager that exercise the basic API and check for error conditions.
    /// </summary>
    /// <author>Mark Pollack</author>
    [TestFixture]
    public class LogManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            LogManager.ResetDefaults();
        }

        [Test]
        public void AdapterProperty()
        {
            ILoggerFactoryAdapter adapter = new NoOpLoggerFactoryAdapter();
            LogManager.Adapter = adapter;
            Assert.AreSame(adapter, LogManager.Adapter);
        }

        [Test]
        public void ConfigureFromConfigurationReader()
        {
            MockRepository mocks = new MockRepository();
            IConfigurationReader r = mocks.StrictMock<IConfigurationReader>();
            using(mocks.Record())
            {
                Expect.Call(r.GetSection(LogManager.COMMON_LOGGING_SECTION)).Return(null);
                Expect.Call(r.GetSection(LogManager.COMMON_LOGGING_SECTION)).Return(new TraceLoggerFactoryAdapter());
                Expect.Call(r.GetSection(LogManager.COMMON_LOGGING_SECTION)).Return(new LogSetting(typeof(ConsoleOutLoggerFactoryAdapter), null));
            }

            using (mocks.Playback())
            {
                ILog log;

                LogManager.ResetDefaults();
                LogManager.ConfigurationReader = r;
                log = LogManager.GetLogger(typeof (LogManagerTests));
                Assert.AreEqual(typeof (NoOpLogger), log.GetType());
                
                LogManager.ResetDefaults();
                LogManager.ConfigurationReader = r;
                log = LogManager.GetLogger(typeof(LogManagerTests));
                Assert.AreEqual(typeof (TraceLogger), log.GetType());

                LogManager.ResetDefaults();
                LogManager.ConfigurationReader = r;
                log = LogManager.GetLogger(typeof(LogManagerTests));
                Assert.AreEqual(typeof (ConsoleOutLogger), log.GetType());
            }
        }

        [Test]
        public void ConfigureFromStandaloneConfig()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging'>
      </factoryAdapter>
    </logging>";
            ILog log = GetLog(xml);
            Assert.IsAssignableFrom(typeof (ConsoleOutLogger), log);
        }


        [Test]
        [ExpectedException(typeof (ConfigurationException))]
        public void InvalidAdapterType()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.Simple.NonExistentAdapter, Common.Logging'>
      </factoryAdapter>
    </logging>";
            ILog log = GetLog(xml);
        }

        [Test]
        [ExpectedException(typeof (ConfigurationException))]
        public void AdapterDoesNotImplementInterface()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.StandaloneConfigurationReader, Common.Logging.Tests'>
      </factoryAdapter>
    </logging>";
            ILog log = GetLog(xml);
        }

        [Test]
        [ExpectedException(typeof (ConfigurationException))]
        public void AdapterDoesNotHaveCorrectCtors()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.MissingCtorFactoryAdapter, Common.Logging.Tests'>
      </factoryAdapter>
    </logging>";
            ILog log = GetLog(xml);
        }

        [Test]
        [ExpectedException(typeof (ConfigurationException))]
        public void AdapterDoesNotHaveCorrectCtorsWithArgs()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.MissingCtorFactoryAdapter, Common.Logging.Tests'>
            <arg key='level' value='DEBUG' />
      </factoryAdapter>
    </logging>";
            ILog log = GetLog(xml);
        }

        [Test]
        public void InvalidXmlSection()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
<foo>
    <logging>
      <factoryAdapter type='Common.Logging.MissingCtorFactoryAdapter, Common.Logging.Tests'>
            <arg key='level' value='DEBUG' />
      </factoryAdapter>
    </logging>
</foo>";
            ILog log = GetLog(xml);
            // lack of proper config section fallsback to no-op logging.
            NoOpLogger noOpLogger = log as NoOpLogger;
            Assert.IsNotNull(noOpLogger);
        }

        private static ILog GetLog(string xml)
        {
            StandaloneConfigurationReader configReader = new StandaloneConfigurationReader(xml);
            LogManager.ConfigurationReader = configReader;
            return LogManager.GetLogger(typeof (LogManagerTests));
        }

        [Test]
        public void GetCurrentClassLoggerUsesCorrectType()
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();
            ConsoleOutLogger log = (ConsoleOutLogger) LogManager.GetCurrentClassLogger();
            Assert.AreEqual(this.GetType().FullName, log.Name);
        }

    }
}