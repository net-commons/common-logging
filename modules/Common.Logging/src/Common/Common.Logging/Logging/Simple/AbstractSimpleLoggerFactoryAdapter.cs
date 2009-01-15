#region License

/*
 * Copyright © 2002-2006 the original author or authors.
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
using System.Collections;
using System.Collections.Specialized;

#endregion

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
    /// <author>Gilles Bayon</author>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    public abstract class AbstractSimpleLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private readonly LogLevel _Level = LogLevel.All;
        private readonly bool _showDateTime = true;
        private readonly bool _showLogName = true;
        private readonly string _dateTimeFormat = string.Empty;

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
            if (properties != null)
            {
                _Level = (LogLevel)ConfigurationHelper.TryParseEnum(_Level, properties["level"]);
                _showDateTime = ConfigurationHelper.TryParseBoolean(_showDateTime, properties["showDateTime"]);
                _showLogName = ConfigurationHelper.TryParseBoolean(_showLogName, properties["showLogName"]);
                _dateTimeFormat = properties["dateTimeFormat"];
            }
        }

        /// <summary>
        /// Create the specified logger instance
        /// </summary>
        protected override ILog CreateLogger(string name)
        {
            return CreateLogger(name, _Level, _showDateTime, _showLogName, _dateTimeFormat);
        }

        /// <summary>
        /// Derived factories need to implement this method to create the
        /// actual logger instance.
        /// </summary>
        /// <returns>a new logger instance. Must never be <c>null</c>!</returns>
        protected abstract ILog CreateLogger(string name, LogLevel level, bool showDateTime, bool showLogName, string dateTimeFormat);
    }
}
