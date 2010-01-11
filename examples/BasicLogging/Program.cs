using Common.Logging;
using Common.Logging.Simple;

namespace BasicLogging
{
    public class Program
    {
        static void Main()
        {
            // set logger factory
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();

            // obtain logger instance
            ILog log = LogManager.GetCurrentClassLogger();

            // log something
            log.Debug("Some Debug Log Output");
        }
    }
}
