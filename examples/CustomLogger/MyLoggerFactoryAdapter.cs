using Common.Logging;
using Common.Logging.Factory;

namespace CustomLogger
{
    public class MyLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private readonly LogLevel _useLogLevel;

        public MyLoggerFactoryAdapter(LogLevel useLogLevel) : base(false)
        {
            _useLogLevel = useLogLevel;    
        }

        protected override ILog CreateLogger(string name)
        {            
            return new MyLogger(name, _useLogLevel);
        }
    }
}