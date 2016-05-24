using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public class ETWLoggerFactoryAdapter: AbstractCachingLoggerFactoryAdapter
    {
        protected override ILog CreateLogger(string name)
        {
            return new ETWLogger();
        }
    }
}