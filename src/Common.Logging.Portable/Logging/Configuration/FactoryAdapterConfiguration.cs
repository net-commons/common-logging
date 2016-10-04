namespace Common.Logging.Configuration
{
    /// <summary>
    /// JSON serializable object representing the configuration of the <see cref="ILoggerFactoryAdapter"/>.
    /// </summary>
    public class FactoryAdapterConfiguration
    {
        /// <summary>
        /// Fully qualified type name of a class implementing <see cref="ILoggerFactoryAdapter"/>.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Optional set of arguments for the constructor of the class specified in <see cref="Type"/>.
        /// </summary>
        public NameValueCollection Arguments { get; set; }
    }
}
