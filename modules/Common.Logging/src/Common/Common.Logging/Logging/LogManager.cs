#region License

/*
 * Copyright © 2002-2008 the original author or authors.
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

#region Imports

using System;
using System.Diagnostics;
using Common.Logging.Simple;

#endregion

namespace Common.Logging
{
    /// <summary>
    /// The LogManager can produce ILogFactory for various logging APIs,
    /// most notably for log4net. 
    /// Other implemenations such as
    /// * SimpleLogger
    /// * NoOpLogger are also supported.
    /// </summary>
    /// <author>Gilles Bayon</author>
    public sealed class LogManager
    {
        private static ILoggerFactoryAdapter _adapter = null;
        private static readonly object _loadLock = new object();
        private static IConfigurationReader _configurationReader = new ConfigurationReader();
        private static readonly string COMMON_SECTION_LOGGING = "common/logging";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager" /> class. 
        /// </summary>
        /// <remarks>
        /// Uses a private access modifier to prevent instantiation of this class.
        /// </remarks>
        private LogManager()
        {
        }


        /// <summary>
        /// Gets or sets the adapter.
        /// </summary>
        /// <value>The adapter.</value>
        public static ILoggerFactoryAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    lock (_loadLock)
                    {
                        if (_adapter == null)
                        {
                            _adapter = BuildLoggerFactoryAdapter();
                        }
                    }
                }
                return _adapter;
            }
            set
            {
                lock (_loadLock)
                {
                    _adapter = value;
                }
            }
        }


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ILog GetLogger(Type type)
        {
            return Adapter.GetLogger(type);
        }


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static ILog GetLogger(string name)
        {
            return Adapter.GetLogger(name);
        }


        /// <summary>
        /// Builds the logger factory adapter.
        /// </summary>
        /// <returns></returns>
        private static ILoggerFactoryAdapter BuildLoggerFactoryAdapter()
        {
            LogSetting setting = null;
            try
            {
                setting = (LogSetting) ConfigurationReader.GetSection(COMMON_SECTION_LOGGING);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException("Could not configure Common.Logging from configuration section 'common/logging'.", ex);
            }

            if (setting != null && !typeof (ILoggerFactoryAdapter).IsAssignableFrom(setting.FactoryAdapterType))
            {
                throw new ConfigurationException(
                    "Specified FactoryAdapter does not implement ILoggerFactoryAdapter.  Check implementation of class " +
                    setting.FactoryAdapterType.AssemblyQualifiedName);
            }

            ILoggerFactoryAdapter instance = null;

            if (setting != null)
            {
                try
                {
                    if (setting.Properties.Count > 0)
                    {
                        object[] args = {setting.Properties};

                        instance =
                            (ILoggerFactoryAdapter) Activator.CreateInstance(setting.FactoryAdapterType, args);
                    }
                    else
                    {
                        instance = (ILoggerFactoryAdapter) Activator.CreateInstance(setting.FactoryAdapterType);
                    }
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(
                        "Unable to create instance of type " + setting.FactoryAdapterType.FullName + 
                        ". Possible explanation is lack of zero arg and single arg NameValueCollection constructors", ex);
                }
            }
            else
            {
                ILoggerFactoryAdapter defaultFactory = new NoOpLoggerFactoryAdapter();
                Trace.WriteLine("Unable to read configuration section common/logging.  Using no-op implemenation.");
                return defaultFactory;               
            }

            return instance;
        }

        /// <summary>
        /// Gets or sets the configuration reader.
        /// </summary>
        /// <remarks>Primarily used for testing purposes but maybe useful to obtain configuration
        /// information from some place other than the .NET application configuration file.</remarks>
        /// <value>The configuration reader.</value>
        public static IConfigurationReader ConfigurationReader
        {
            get
            {
                return _configurationReader;
            }
            set
            {
                _configurationReader = value;
            }
        }

    }
}