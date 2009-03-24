#region License

/*
 * Copyright © 2002-2009 the original author or authors.
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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Common.Logging.Simple;

namespace Common.Logging
{
    /// <summary>
    /// Use the LogManager's <see cref="GetLogger(string)"/> or <see cref="GetLogger(System.Type)"/> 
    /// methods to obtain <see cref="ILog"/> instances for logging.
    /// </summary>
    /// <remarks>
    /// For configuring the underlying log system using application configuration, see the example 
    /// at <see cref="ConfigurationSectionHandler"/>. 
    /// For configuring programmatically, see the example section below.
    /// </remarks>
    /// <example>
    /// The example below shows the typical use of LogManager to obtain a reference to a logger
    /// and log an exception:
    /// <code>
    /// 
    /// ILog log = LogManager.GetLogger(this.GetType());
    /// ...
    /// try 
    /// { 
    ///   /* .... */ 
    /// }
    /// catch(Exception ex)
    /// {
    ///   log.ErrorFormat("Hi {0}", ex, "dude");
    /// }
    /// 
    /// </code>
    /// The example below shows programmatic configuration of the underlying log system:
    /// <code>
    /// 
    /// // create properties
    /// NameValueCollection properties = new NameValueCollection();
    /// properties[&quot;showDateTime&quot;] = &quot;true&quot;;
    /// 
    /// // set Adapter
    /// Common.Logging.LogManager.Adapter = new 
    /// Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(properties);
    /// 
    /// </code>
    /// </example>
    /// <seealso cref="ILog"/>
    /// <seealso cref="Adapter"/>
    /// <seealso cref="ILoggerFactoryAdapter"/>
    /// <seealso cref="ConfigurationSectionHandler"/>
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
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the type of the calling class.
        /// </summary>
        /// <seealso cref="GetLogger(Type)"/>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ILog GetCurrentClassLogger()
        {
            StackFrame frame = new StackFrame(1, false);
            return Adapter.GetLogger(frame.GetMethod().DeclaringType);
        }

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        public static ILog GetLogger(Type type)
        {
            return Adapter.GetLogger(type);
        }


        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(string)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
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