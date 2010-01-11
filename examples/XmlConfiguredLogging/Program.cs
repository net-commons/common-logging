using Common.Logging;

namespace XmlConfiguredLogging
{
    public class Program
    {
        static void Main()
        {
            // obtain logger instance
            ILog log = LogManager.GetCurrentClassLogger();

            // log something
            log.Info("Some Debug Log Output");
        }
    }
}
