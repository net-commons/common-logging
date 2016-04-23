using NUnit.Framework;

namespace Common.Logging.MultipleLogger.Tests.Logging.MultipleLogger
{
    [TestFixture]
    public class FileBasedConfigurationTests
    {
        [Test]
        public void MyMethod()
        {
            var multiLogger = LogManager.GetLogger<FileBasedConfigurationTests>();


        }
    }
}