using System;

namespace Common.Logging.Configuration
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationReader"/> that uses a supplied
    /// <see cref="LogConfiguration"/> object.
    /// </summary>
    /// <author>Brant Burnett</author>
    public class LogConfigurationReader : IConfigurationReader
    {
        private readonly LogConfiguration _configuration;

        /// <summary>
        /// Creates a new <see cref="LogConfigurationReader"/> given a <see cref="LogConfiguration"/> object.
        /// </summary>
        /// <param name="configuration"><see cref="LogConfiguration"/> to be parsed.</param>
        public LogConfigurationReader(LogConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _configuration = configuration;
        }

        /// <summary>
        /// Returns a <see cref="LogSetting"/> based on the <see cref="LogConfiguration"/> supplied
        /// in the constructor.
        /// </summary>
        /// <param name="sectionName">This parameter is not used in this implementation.</param>
        /// <returns><see cref="LogSetting"/> based on the supplied configuration.</returns>
        public object GetSection(string sectionName)
        {
            if (_configuration.FactoryAdapter == null)
            {
                throw new ConfigurationException("LogConfiguration.FactoryAdapter is required.");
            }
            if (string.IsNullOrEmpty(_configuration.FactoryAdapter.Type))
            {
                throw new ConfigurationException("LogConfiguration.FactoryAdapter.Type is required.");
            }

            Type factoryType;
            try
            {
                factoryType = Type.GetType(_configuration.FactoryAdapter.Type, true);
            }
            catch (Exception e)
            {
                throw new ConfigurationException
                  ("Unable to create type '" + _configuration.FactoryAdapter.Type + "'"
                    , e
                  );
            }

            return new LogSetting(factoryType, _configuration.FactoryAdapter.Arguments);
        }
    }
}
