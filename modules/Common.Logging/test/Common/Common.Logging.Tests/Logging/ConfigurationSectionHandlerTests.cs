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

using Common.Logging.Simple;
using NUnit.Framework;

namespace Common.Logging
{
    [TestFixture]
    public class ConfigurationSectionHandlerTests
    {
        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void TooManyAdapterElements()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging'>
      </factoryAdapter>
      <factoryAdapter type='Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging'>
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            reader.GetSection(null);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoTypeElementForAdapterDeclaration()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter clazz='Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging'>
        <arg kez='level' value='DEBUG' />
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            reader.GetSection(null);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoKeyElementForAdapterArguments()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging'>
        <arg kez='level' value='DEBUG' />
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            reader.GetSection(null);
        }

        [Test]
        public void ConsoleShortCut()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='CONSOLE'/>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);
            Assert.AreEqual(
                "ConsoleOutLoggerFactoryAdapter", setting.FactoryAdapterType.Name);

        }

        [Test]
        public void TraceShortCut()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='TRACE'/>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);
            Assert.AreEqual(
                "TraceLoggerFactoryAdapter", setting.FactoryAdapterType.Name);

        }

        [Test]
        public void NoOpShortCut()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='NOOP'/>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader(xml);
            LogSetting setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);
            Assert.AreEqual(
                "NoOpLoggerFactoryAdapter", setting.FactoryAdapterType.Name);

        }
    }
}
