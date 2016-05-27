using Common.Logging.Configuration;
using Common.Logging.ETW;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class ETWLoggerTests
    {
        [Test]
        public void ExplicitLoggingLevelImplicitlyIncludesGreaterLoggingLevels()
        {
            Assume.That(LogLevel.Fatal > LogLevel.Debug, "Relationship between FATAL and DEBUG logging levels not as expected.");

            var props = new NameValueCollection { { "level", "debug" } };
            var adapter = new ETWLoggerFactoryAdapter(props);

            var logger = adapter.GetLogger(string.Empty);

            Assert.That(logger.IsFatalEnabled);
        }
    }
}