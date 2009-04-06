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
using Common.Logging.Configuration;

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
        /// Initializes a new instance of the <see cref="AbstractSimpleLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>
        /// Looks for level, showDateTime, showLogName, dateTimeFormat items from 
        /// <paramref name="properties" /> for use when the GetLogger methods are called.
        /// <see cref="ConfigurationSectionHandler"/> for more information on how to use the 
        /// standard .NET application configuraiton file (App.config/Web.config) 
        /// to configure this adapter.
        /// </remarks>
        /// <param name="properties">The name value collection, typically specified by the user in 
        /// a configuration section named common/logging.</param>
        protected AbstractSimpleLoggerFactoryAdapter(NameValueCollection properties)
            : base(true)
        {
            _level = ArgUtils.TryParseEnum(LogLevel.All, ArgUtils.GetValue(properties, "level"));
            _showDateTime = ArgUtils.TryParse(true, ArgUtils.GetValue(properties, "showDateTime"));
            _showLogName = ArgUtils.TryParse(true, ArgUtils.GetValue(properties, "showLogName"));
            _showLevel = ArgUtils.TryParse(true, ArgUtils.GetValue(properties, "showLevel"));
            _dateTimeFormat = ArgUtils.GetValue(properties, "dateTimeFormat", string.Empty);
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
