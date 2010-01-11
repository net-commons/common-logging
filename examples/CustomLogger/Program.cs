using Common.Logging;

namespace CustomLogger
{
    public class Program
    {
        static void Main(string[] args)
        {
            // set our custom factory adapter
            LogManager.Adapter = new MyLoggerFactoryAdapter(LogLevel.All);

            // obtain logger instance
            ILog log = LogManager.GetCurrentClassLogger();

            // log something
            log.Debug("Some Debug Log Output");
        }
    }
}
