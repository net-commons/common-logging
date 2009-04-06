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

using System.Configuration;
using Common.Logging.Configuration;
using Common.Logging.Simple;
using NUnit.Framework;

namespace Common.Logging
{
    [TestFixture]
    public class ConfigurationSectionHandlerTests
    {
        [Test]
        public void NoParentSectionsAllowed()
        {
            IConfigurationSectionHandler handler = new ConfigurationSectionHandler();
            Assert.Throws(Is.TypeOf<ConfigurationException>().And.Message.EqualTo("parent configuration sections are not allowed")
                         , delegate {
                                  handler.Create(new LogSetting(typeof (ConsoleOutLoggerFactoryAdapter), null), 
                                                 null,
                                                 null);
                          });
        }

        [Test]
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
            Assert.Throws( Is.TypeOf<ConfigurationException>()
                            .And.Message.EqualTo("Only one <factoryAdapter> element allowed")
                            , delegate {
                            reader.GetSection(null);
                        });
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
            Assert.AreEqual(typeof(ConsoleOutLoggerFactoryAdapter), setting.FactoryAdapterType);

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
            Assert.AreEqual(typeof(TraceLoggerFactoryAdapter), setting.FactoryAdapterType);

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
            Assert.AreEqual(typeof(NoOpLoggerFactoryAdapter), setting.FactoryAdapterType);

        }

        [Test]
        public void ArgumentKeysCaseInsensitive()
        {
            const string xml =
    @"<?xml version='1.0' encoding='UTF-8' ?>
    <logging>
      <factoryAdapter type='CONSOLE'>
        <arg key='LeVel' value='DEBUG' />
        <arg key='LEVEL' value='DEBUG' />
        <arg key='level' value='DEBUG' />
      </factoryAdapter>
    </logging>";
            StandaloneConfigurationReader reader = new StandaloneConfigurationReader( xml );
            LogSetting setting = reader.GetSection( null ) as LogSetting;
            Assert.IsNotNull( setting );
            Assert.AreEqual( 1, setting.Properties.Count );
            Assert.AreEqual( 3, setting.Properties.GetValues("LeVeL").Length );
        }
    }
}
