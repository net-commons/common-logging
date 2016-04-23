#region License

/*
 * Copyright 2002-2009 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion
 
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
