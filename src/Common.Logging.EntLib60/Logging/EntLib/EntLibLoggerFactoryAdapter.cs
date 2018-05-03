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
using Common.Logging.Configuration;
using Common.Logging.Factory;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Adapts the EnterpriseLibrary 6.0 logging system to Common.Logging.
    /// </summary>
    /// <remarks>
    /// The following configuration property values may be configured:
    /// <list type="bullet">
    ///     <item>DefaultPriority (see <see cref="DefaultPriority"/>)</item>
    ///     <item>ExceptionFormat (see <see cref="ExceptionFormat"/>)</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following snippet shows how to configure EntLib logging for Common.Logging:
    /// <code>
    /// &lt;configuration&gt;
    ///   &lt;configSections&gt;
    ///       &lt;section name=&quot;logging&quot; type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot; /&gt;
    ///       &lt;section name=&quot;loggingConfiguration&quot; type=&quot;Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LoggingSettings, Microsoft.Practices.EnterpriseLibrary.Logging, Version=4.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35&quot; /&gt;
    ///   &lt;/configSections&gt;
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;Common.Logging.EntLib.EntLibLoggerFactoryAdapter, Common.Logging.EntLib41&quot;&gt;
    ///         &lt;arg key=&quot;DefaultPriority&quot; value=&quot;-1&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    ///   &lt;loggingConfiguration name=&quot;Logging Application Block&quot;
    /// &lt;-- configure enterprise logging application block here --&gt;
    /// ...
    ///   &lt;/loggingConfiguration&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    public class EntLibLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private readonly EntLibLoggerSettings _settings;
        private LogWriter _logWriter;

        /// <summary>
        /// The default priority used to log events.
        /// </summary>
        /// <remarks>defaults to <see cref="EntLibLoggerSettings.DEFAULTPRIORITY"/></remarks>
        public int DefaultPriority
        {
            get { return _settings.priority; }
        }

        /// <summary>
        /// The format string used for formatting exceptions
        /// </summary>
        /// <remarks>
        /// defaults to <see cref="EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT"/>
        /// </remarks>
        public string ExceptionFormat
        {
            get { return _settings.exceptionFormat; }
        }

        /// <summary>
        /// the <see cref="_logWriter"/> to write log events to.
        /// </summary>
        /// <remarks>
        /// defaults to <see cref="Logger.Writer"/>.
        /// </remarks>
        public LogWriter LogWriter
        {
            get
            {
                if (_logWriter == null)
                {
                    lock (this)
                    {
                        if (_logWriter == null)
                        {
                            _logWriter = GetWriter();
                        }
                    }
                }
                return _logWriter;
            }
        }

        private LogWriter GetWriter()
        {
            //per http://growingtech.blogspot.it/2013/05/enterprise-library-60-logwriter-has-not.html 
            //  we have to explicitly set the writer for EntLib6 before we can return it
            var configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());

            return Logger.Writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        public EntLibLoggerFactoryAdapter()
            : this(
                EntLibLoggerSettings.DEFAULTPRIORITY, EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT, null,
                EntLibLoggerSettings.DEFAULTLOGCATEGORY)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class
        /// with the specified configuration parameters.
        /// </summary>
        /// <param name="defaultPriority">defaults to <see cref="EntLibLoggerSettings.DEFAULTPRIORITY"/></param>
        /// <param name="exceptionFormat">defaults to <see cref="EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT"/></param>
        /// <param name="logWriter">a <see cref="LogWriter"/> instance to use</param>
        /// <param name="logCategory">defaults to <see cref="EntLibLoggerSettings.DEFAULTLOGCATEGORY"/></param>
        public EntLibLoggerFactoryAdapter(int defaultPriority, string exceptionFormat, LogWriter logWriter,
            string logCategory)
            : base(true)
        {
            if (exceptionFormat.Length == 0)
            {
                exceptionFormat = null;
            }
            _settings = new EntLibLoggerSettings(defaultPriority, exceptionFormat, logCategory);
            _logWriter = logWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>passed in values are not used, configuration is external to EntLib logging API</remarks>
        /// <param name="properties">The properties.</param>
        public EntLibLoggerFactoryAdapter(NameValueCollection properties)
            : this(ArgUtils.TryParse(EntLibLoggerSettings.DEFAULTPRIORITY, ArgUtils.GetValue(properties, "priority"))
                , ArgUtils.Coalesce(ArgUtils.GetValue(properties, "exceptionFormat"), EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT)
                , null
                , ArgUtils.Coalesce(ArgUtils.GetValue(properties, "logCategory"), EntLibLoggerSettings.DEFAULTLOGCATEGORY)
            )
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntLibLogger"/> instance.
        /// </summary>
        protected override ILog CreateLogger(string name)
        {
            var logCategory = String.IsNullOrWhiteSpace(_settings.logCategory) ? name : _settings.logCategory;
            return CreateLogger(logCategory, LogWriter, _settings);
        }

        /// <summary>
        /// Creates a new <see cref="EntLibLogger"/> instance.
        /// </summary>
        protected virtual ILog CreateLogger(string name, LogWriter logWriter, EntLibLoggerSettings settings)
        {
            return new EntLibLogger(name, LogWriter, settings);
        }
    }
}