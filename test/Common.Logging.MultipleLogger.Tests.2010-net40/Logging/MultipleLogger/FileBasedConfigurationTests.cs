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