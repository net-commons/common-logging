using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public class ETWLoggerFactoryAdapter: AbstractCachingLoggerFactoryAdapter
    {

        public ICommonLoggingEventSource ETWEventSource { get; set; }

        public ETWLoggerFactoryAdapter()
        {
            ETWEventSource = new CommonLoggingEventSource();
        }

        protected override ILog CreateLogger(string name)
        {
            return new ETWLogger(ETWEventSource);
        }
    }
}