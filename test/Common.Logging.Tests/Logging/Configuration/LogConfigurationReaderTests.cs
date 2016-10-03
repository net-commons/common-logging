#region License

/*
 * Copyright 2002-2009 the original author or authors.
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

using System;
using NUnit.Framework;

namespace Common.Logging.Configuration
{
    /// <summary>
    /// </summary>
    /// <author>Brant Burnett</author>
    [TestFixture]
    public class LogConfigurationReaderTests
    {
        [Test]
        public void ctor_NullConfiguration_ThrowsArgumentNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => {
                var reader = new LogConfigurationReader(null);
            });

            Assert.AreEqual("configuration", ex.ParamName);
        }

        [Test]
        public void GetSection_NullFactoryAdapter_ThrowsConfigurationException()
        {
            var config = new LogConfiguration();

            var reader = new LogConfigurationReader(config);

            Assert.Throws<ConfigurationException>(() => reader.GetSection(null));
        }

        [Test]
        public void GetSection_NullFactoryAdapterType_ThrowsConfigurationException()
        {
            var config = new LogConfiguration()
            {
                FactoryAdapter = new FactoryAdapterConfiguration()
            };

            var reader = new LogConfigurationReader(config);

            Assert.Throws<ConfigurationException>(() => reader.GetSection(null));
        }

        [Test]
        public void GetSection_EmptyFactoryAdapterType_ThrowsConfigurationException()
        {
            var config = new LogConfiguration()
            {
                FactoryAdapter = new FactoryAdapterConfiguration()
                {
                    Type = ""
                }
            };

            var reader = new LogConfigurationReader(config);

            Assert.Throws<ConfigurationException>(() => reader.GetSection(null));
        }

        [Test]
        public void GetSection_BadFactoryAdapterType_ThrowsConfigurationException()
        {
            var config = new LogConfiguration()
            {
                FactoryAdapter = new FactoryAdapterConfiguration()
                {
                    Type = "SomeType, SomeAssembly"
                }
            };

            var reader = new LogConfigurationReader(config);

            Assert.Throws<ConfigurationException>(() => reader.GetSection(null));
        }

        [Test]
        public void GetSection_GoodFactoryAdapterType_ReturnsLogSettingWithType()
        {
            var config = new LogConfiguration()
            {
                FactoryAdapter = new FactoryAdapterConfiguration()
                {
                    Type = typeof(FakeFactoryAdapter).AssemblyQualifiedName
                }
            };

            var reader = new LogConfigurationReader(config);

            var result = reader.GetSection(null) as LogSetting;

            Assert.NotNull(result);
            Assert.AreEqual(typeof(FakeFactoryAdapter), result.FactoryAdapterType);
        }

        [Test]
        public void GetSection_NoArguments_ReturnsLogSettingWithNullArguments()
        {
            var config = new LogConfiguration()
            {
                FactoryAdapter = new FactoryAdapterConfiguration()
                {
                    Type = typeof(FakeFactoryAdapter).AssemblyQualifiedName
                }
            };

            var reader = new LogConfigurationReader(config);

            var result = reader.GetSection(null) as LogSetting;

            Assert.NotNull(result);
            Assert.IsNull(result.Properties);
        }

        [Test]
        public void GetSection_HasArguments_ReturnsLogSettingWithArguments()
        {
            var config = new LogConfiguration()
            {
                FactoryAdapter = new FactoryAdapterConfiguration()
                {
                    Type = typeof(FakeFactoryAdapter).AssemblyQualifiedName,
                    Arguments = new NameValueCollection
                    {
                        { "arg1", "value1" }
                    }
                }
            };

            var reader = new LogConfigurationReader(config);

            var result = reader.GetSection(null) as LogSetting;

            Assert.NotNull(result);
            Assert.AreEqual("value1", result.Properties["arg1"]);
        }

        #region Helpers

        public class FakeFactoryAdapter : ILoggerFactoryAdapter
        {
            public ILog GetLogger(Type type)
            {
                throw new NotImplementedException();
            }

            public ILog GetLogger(string key)
            {
                throw new NotImplementedException();
            }
        }

#endregion
    }
}