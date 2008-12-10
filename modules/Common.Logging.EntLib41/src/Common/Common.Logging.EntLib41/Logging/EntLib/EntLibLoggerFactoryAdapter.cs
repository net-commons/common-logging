#region License

/*
 * Copyright 2002-2007 the original author or authors.
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

using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging;

#endregion

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Adapts the EnterpriseLibrary logging system to Common.Logging.
    /// </summary>
    /// <remarks>
    /// The following configuration property values may be configured:
    /// <list type="bullet">
    ///     <item>DefaultPriority (see <see cref="DefaultPriority"/>)</item>
    ///     <item>ExceptionFormat (see <see cref="ExceptionFormat"/>)</item>
    /// </list>
    /// </remarks>
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
                            _logWriter = Logger.Writer;
                        }
                    }
                }
                return _logWriter;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        public EntLibLoggerFactoryAdapter()
            : this(EntLibLoggerSettings.DEFAULTPRIORITY, EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class
        /// with the specified configuration parameters.
        /// </summary>
        public EntLibLoggerFactoryAdapter(int defaultPriority, string exceptionFormat, LogWriter logWriter)
            :base(true)
        {            
            if (exceptionFormat.Length == 0)
            {
                exceptionFormat = null;
            }
            _settings = new EntLibLoggerSettings(defaultPriority, exceptionFormat);
            _logWriter = logWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>passed in values are not used, configuration is external to EntLib logging API</remarks>
        /// <param name="properties">The properties.</param>
        public EntLibLoggerFactoryAdapter(NameValueCollection properties)
            : this(ConfigurationUtils.TryParseInt(EntLibLoggerSettings.DEFAULTPRIORITY, ConfigurationUtils.GetValue(properties, "priority"))
                 , ConfigurationUtils.Coalesce(ConfigurationUtils.GetValue(properties, "exceptionFormat"), EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT)
                 , null
            )
        {}

        /// <summary>
        /// Creates a new <see cref="EntLibLogger"/> instance.
        /// </summary>
        protected override ILog CreateLogger(string name)
        {
            return CreateLogger(name, LogWriter, _settings);            
        }

        /// <summary>
        /// Creates a new <see cref="EntLibLogger"/> instance.
        /// </summary>
        protected virtual ILog CreateLogger(string name, LogWriter logWriter, EntLibLoggerSettings settings)
        {
            return new EntLibLogger(name, LogWriter, _settings);                        
        }
    }
}