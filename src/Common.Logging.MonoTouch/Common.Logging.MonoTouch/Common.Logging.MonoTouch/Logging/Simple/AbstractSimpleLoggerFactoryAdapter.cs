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

using System.Collections.Specialized;
using Common.Logging.Factory;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Base factory implementation for creating simple <see cref="ILog" /> instances.
    /// </summary>
    /// <remarks>Default settings are LogLevel.All, showDateTime = true, showLogName = true, and no DateTimeFormat.
    /// The keys in the NameValueCollection to configure this adapter are the following
    /// <list type="bullet">
    ///     <item>level</item>
    ///     <item>showDateTime</item>
    ///     <item>showLogName</item>
    ///     <item>dateTimeFormat</item>
    /// </list>
    /// <example>
    /// Here is an example how to implement your own logging adapter:
    /// <code>
    /// public class ConsoleOutLogger : AbstractSimpleLogger
    /// {
    ///   public ConsoleOutLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, 
    /// bool showLogName, string dateTimeFormat)
    ///       : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
    ///   {
    ///   }
    /// 
    ///   protected override void WriteInternal(LogLevel level, object message, Exception e)
    ///   {
    ///       // Use a StringBuilder for better performance
    ///       StringBuilder sb = new StringBuilder();
    ///       FormatOutput(sb, level, message, e);
    /// 
    ///       // Print to the appropriate destination
    ///       Console.Out.WriteLine(sb.ToString());
    ///   }
    /// }
    /// 
    /// public class ConsoleOutLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    /// {
    ///   public ConsoleOutLoggerFactoryAdapter(NameValueCollection properties)
    ///       : base(properties)
    ///   { }
    /// 
    ///   protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool 
    /// showDateTime, bool showLogName, string dateTimeFormat)
    ///   {
    ///       ILog log = new ConsoleOutLogger(name, level, showLevel, showDateTime, showLogName, 
    /// dateTimeFormat);
    ///       return log;
    ///   }
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    /// <seealso cref="LogManager.Adapter"/>
    /// <seealso cref="ConfigurationSectionHandler"/>
    /// <author>Gilles Bayon</author>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    public abstract class AbstractSimpleLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private LogLevel _level;
        private bool _showLevel;
        private bool _showDateTime;
        private bool _showLogName;
        private string _dateTimeFormat;

        /// <summary>
        /// The default <see cref="LogLevel"/> to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        [CoverageExclude]
        public LogLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        [CoverageExclude]
        public bool ShowLevel
        {
            get { return _showLevel; }
            set { _showLevel = value; }
        }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        [CoverageExclude]
        public bool ShowDateTime
        {
            get { return _showDateTime; }
            set { _showDateTime = value; }
        }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        [CoverageExclude]
        public bool ShowLogName
        {
            get { return _showLogName; }
            set { _showLogName = value; }
        }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        [CoverageExclude]
        public string DateTimeFormat
        {
            get { return _dateTimeFormat; }
            set { _dateTimeFormat = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSimpleLoggerFactoryAdapter"/> class with 
        /// default settings for the loggers created by this factory.
        /// </summary>
        protected AbstractSimpleLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel, string dateTimeFormat)
            :base(true)
        {
            _level = level;
            _showDateTime = showDateTime;
            _showLogName = showLogName;
            _showLevel = showLevel;
            _dateTimeFormat = dateTimeFormat ?? string.Empty;            
        }

        /// <summary>
        /// Create the specified logger instance
        /// </summary>
        protected override ILog CreateLogger(string name)
        {
            return CreateLogger(name, _level, _showLevel, _showDateTime, _showLogName, _dateTimeFormat);
        }

        /// <summary>
        /// Derived factories need to implement this method to create the
        /// actual logger instance.
        /// </summary>
        /// <returns>a new logger instance. Must never be <c>null</c>!</returns>
        protected abstract ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat);
    }
}
