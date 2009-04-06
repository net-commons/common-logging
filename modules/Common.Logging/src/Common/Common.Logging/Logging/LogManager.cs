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
using Common.Logging.Configuration;

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
    public static class LogManager
    {
        /// <summary>
        /// The name of the default configuration section to read settings from.
        /// </summary>
        /// <remarks>
        /// You can always change the source of your configuration settings by setting another <see cref="IConfigurationReader"/> instance
        /// on <see cref="ConfigurationReader"/>.
        /// </remarks>
        public static readonly string COMMON_LOGGING_SECTION = "common/logging";

        private static IConfigurationReader _configurationReader;
        private static ILoggerFactoryAdapter _adapter;
        private static readonly object _loadLock = new object();

        /// <summary>
        /// Performs static 1-time init of LogManager by calling <see cref="Reset()"/>
        /// </summary>
        static LogManager()
        {
            Reset();
        }

        /// <summary>
        /// Reset the <see cref="Common.Logging" /> infrastructure to its default settings. This means, that configuration settings
        /// will be re-read from section <c>&lt;common/logging&gt;</c> of your <c>app.config</c>.
        /// </summary>
        /// <remarks>
        /// This is mainly used for unit testing, you wouldn't normally use this in your applications.<br/>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected. 
        /// Resetting LogManager only affects new instances being handed out.
        /// </remarks>
        public static void Reset()
        {
            Reset(new DefaultConfigurationReader());
        }

        /// <summary>
        /// Reset the <see cref="Common.Logging" /> infrastructure to its default settings. This means, that configuration settings
        /// will be re-read from section <c>&lt;common/logging&gt;</c> of your <c>app.config</c>.
        /// </summary>
        /// <remarks>
        /// This is mainly used for unit testing, you wouldn't normally use this in your applications.<br/>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected. 
        /// Resetting LogManager only affects new instances being handed out.
        /// </remarks>
        /// <param name="reader">
        /// the <see cref="IConfigurationReader"/> instance to obtain settings for 
        /// re-initializing the LogManager.
        /// </param>
        public static void Reset(IConfigurationReader reader)
        {
            lock (_loadLock)
            {
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }
                _configurationReader = reader;
                _adapter = null;
            }
        }

        /// <summary>
        /// Gets the configuration reader used to initialize the LogManager.
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
                if (value == null)
                {
                    throw new ArgumentNullException("Adapter");
                }

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
        /// <remarks>
        /// This method needs to inspect the <see cref="StackTrace"/> in order to determine the calling 
        /// class. This of course comes with a performance penalty, thus you shouldn't call it too
        /// often in your application.
        /// </remarks>
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
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        public static ILog GetLogger<T>()
        {
            return Adapter.GetLogger(typeof(T));
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
        /// <returns>a factory adapter instance. Is never <c>null</c>.</returns>
        private static ILoggerFactoryAdapter BuildLoggerFactoryAdapter()
        {
            object sectionResult = null;

            ArgUtils.Guard(delegate
                      {
                          sectionResult = ConfigurationReader.GetSection(COMMON_LOGGING_SECTION);
                      }
                , "Failed obtaining configuration for Common.Logging from configuration section 'common/logging'.");

            // configuration reader returned <null>
            if (sectionResult == null)
            {
                string message = (ConfigurationReader.GetType() == typeof(DefaultConfigurationReader))
                                     ? string.Format("no configuration section <{0}> found - suppressing logging output", COMMON_LOGGING_SECTION)
                                     : string.Format("Custom ConfigurationReader '{0}' returned <null> - suppressing logging output", ConfigurationReader.GetType().FullName);
                Trace.WriteLine(message);
                ILoggerFactoryAdapter defaultFactory = new NoOpLoggerFactoryAdapter();
                return defaultFactory;
            }

            // ready to use ILoggerFactoryAdapter?
            if (sectionResult is ILoggerFactoryAdapter)
            {
                Trace.WriteLine(string.Format("Using ILoggerFactoryAdapter returned from custom ConfigurationReader '{0}'", ConfigurationReader.GetType().FullName));
                return (ILoggerFactoryAdapter)sectionResult;
            }

            // ensure what's left is a LogSetting instance
            ArgUtils.Guard(delegate
                               {
                                   ArgUtils.AssertIsAssignable<LogSetting>("sectionResult", sectionResult.GetType());
                               }
                           , "ConfigurationReader {0} returned unknown settings instance of type {1}"
                           , ConfigurationReader.GetType().FullName, sectionResult.GetType().FullName);

            ILoggerFactoryAdapter adapter = null;
            ArgUtils.Guard(delegate
                    {
                        adapter = BuildLoggerFactoryAdapterFromLogSettings((LogSetting)sectionResult);
                    }
                , "Failed creating LoggerFactoryAdapter from settings");

            return adapter;
        }

        /// <summary>
        /// Builds a <see cref="ILoggerFactoryAdapter"/> instance from the given <see cref="LogSetting"/>
        /// using <see cref="Activator"/>.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>the <see cref="ILoggerFactoryAdapter"/> instance. Is never <c>null</c></returns>
        private static ILoggerFactoryAdapter BuildLoggerFactoryAdapterFromLogSettings(LogSetting setting)
        {
            ArgUtils.AssertNotNull("setting", setting);
            // already ensured by LogSetting
            //            AssertArgIsAssignable<ILoggerFactoryAdapter>("setting.FactoryAdapterType", setting.FactoryAdapterType
            //                                , "Specified FactoryAdapter does not implement {0}.  Check implementation of class {1}"
            //                                , typeof(ILoggerFactoryAdapter).FullName
            //                                , setting.FactoryAdapterType.AssemblyQualifiedName);

            ILoggerFactoryAdapter adapter = null;

            ArgUtils.Guard(delegate
                    {
                        if (setting.Properties != null
                            && setting.Properties.Count > 0)
                        {
                            object[] args = { setting.Properties };

                            adapter = (ILoggerFactoryAdapter)Activator.CreateInstance(setting.FactoryAdapterType, args);
                        }
                        else
                        {
                            adapter = (ILoggerFactoryAdapter)Activator.CreateInstance(setting.FactoryAdapterType);
                        }
                    }
                    , "Unable to create instance of type {0}. Possible explanation is lack of zero arg and single arg NameValueCollection constructors"
                    , setting.FactoryAdapterType.FullName
            );

            // make sure
            ArgUtils.AssertNotNull("adapter", adapter, "Activator.CreateInstance() returned <null>");
            return adapter;
        }
    }
}