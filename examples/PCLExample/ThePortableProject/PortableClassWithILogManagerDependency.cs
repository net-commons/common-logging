using Common.Logging;

namespace ThePortableProject
{
    public class PortableClassWithILogManagerDependency
    {
        private readonly ILogManager _logManager;

        public PortableClassWithILogManagerDependency(ILogManager logManager)
        {
            _logManager = logManager;
        }

        public void SomeMethodThatLogs(string message)
        {
            var logger = _logManager.GetLogger(this.GetType());

            logger.Info(message);
        }
    }
}