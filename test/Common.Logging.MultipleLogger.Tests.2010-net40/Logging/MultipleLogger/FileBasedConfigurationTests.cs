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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging.Log4Net;
using Common.Logging.NLog;
using NUnit.Framework;

namespace Common.Logging.MultipleLogger.Tests.Logging.MultipleLogger
{
    [TestFixture]
    public class FileBasedConfigurationTests
    {
        [Test]
        public void CanConfigureMultipleLoggersFromFile()
        {
            var multiLogger = LogManager.GetLogger<FileBasedConfigurationTests>();

            //the only way to test with certainty is to use private reflection to grab the configured loggers...
            var configuredLoggers =
                typeof (MultiLogger).GetField("_loggers", BindingFlags.NonPublic | BindingFlags.Instance)?
                    .GetValue(multiLogger) as List<ILog>;

            Assert.That(configuredLoggers, Is.Not.Null);
            Assert.That(configuredLoggers.Any(logger => logger is Log4NetLogger));
            Assert.That(configuredLoggers.Any(logger => logger is NLogLogger));
        }
    }
}