using Common.Logging.ETW;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class ETWLoggerFactoryAdapterTests : ILogTestsBase
    {
        protected override ILoggerFactoryAdapter GetLoggerFactoryAdapter()
        {
            return new ETWLoggerFactoryAdapter();
        }
    }
}