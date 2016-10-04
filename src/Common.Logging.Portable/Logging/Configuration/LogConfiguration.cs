namespace Common.Logging.Configuration
{
    /// <summary>
    /// JSON serializable object representing the configuration of the logging subsystem.
    /// May be passed to <see cref="LogManager.Configure"/>.
    /// </summary>
    public class LogConfiguration
    {
        /// <summary>
        /// Defines the <see cref="ILoggerFactoryAdapter"/> used by the logging subsystem.
        /// </summary>
        public FactoryAdapterConfiguration FactoryAdapter { get; set; }
    }
}
