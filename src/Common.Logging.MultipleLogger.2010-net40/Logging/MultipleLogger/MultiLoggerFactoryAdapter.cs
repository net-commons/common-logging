using System;
using System.Collections.Generic;
using Common.Logging.Configuration;


namespace Common.Logging.MultipleLogger
{
    /// <summary>
    /// Multi Factory Logger Factory Adapter can use multiple logging factory
    /// implementations to send a log item to multiple logging engines.
    /// </summary>
    public class MultiLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        /// <summary>
        /// Registered Logger Factory Adapters.
        /// </summary>
        public readonly List<ILoggerFactoryAdapter> LoggerFactoryAdapters;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLoggerFactoryAdapter"/> class.
        /// </summary>
        public MultiLoggerFactoryAdapter()
        {
            LoggerFactoryAdapters = new List<ILoggerFactoryAdapter>();
        }


        public MultiLoggerFactoryAdapter(NameValueCollection properties) : this()
        {
            var reader = new DefaultConfigurationReader();

            var childFactoryAdapterSettings = reader.GetSection("common/logging.multipleLoggers");

            if (!(childFactoryAdapterSettings is List<LogSetting>))
                return;

            foreach (var factoryAdapterSetting in childFactoryAdapterSettings as List<LogSetting>)
            {
                LoggerFactoryAdapters.Add(LogManager.BuildLoggerFactoryAdapterFromLogSettings(factoryAdapterSetting));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <param name="factoryAdapters">The factory adapters.</param>
        public MultiLoggerFactoryAdapter(List<ILoggerFactoryAdapter> factoryAdapters)
        {
            LoggerFactoryAdapters = factoryAdapters;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns></returns>
        public ILog GetLogger(string loggerName)
        {
            var loggers = new List<ILog>(LoggerFactoryAdapters.Count);

            foreach (var f in LoggerFactoryAdapters)
            {
                loggers.Add(f.GetLogger(loggerName));
            }

            return new MultiLogger(loggers);
        }

        /// <summary>
        /// Get a ILog instance by type.
        /// </summary>
        /// <param name="type">The type to use for the logger</param>
        /// <returns></returns>
        public ILog GetLogger(Type type)
        {
            var loggers = new List<ILog>(LoggerFactoryAdapters.Count);

            foreach (var f in LoggerFactoryAdapters)
            {
                loggers.Add(f.GetLogger(type));
            }

            return new MultiLogger(loggers);
        }
    }
}
