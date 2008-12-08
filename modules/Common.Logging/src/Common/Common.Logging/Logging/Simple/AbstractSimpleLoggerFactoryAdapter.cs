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
    public abstract class AbstractSimpleLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        private readonly Hashtable _cachedLoggers = new Hashtable();
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
        {
            if (properties != null)
            {
                _Level = (LogLevel)TryParseEnum(_Level, properties["level"]);
                _showDateTime = TryParseBoolean(_showDateTime, properties["showDateTime"]);
                _showLogName = TryParseBoolean(_showLogName, properties["showLogName"]);
                _dateTimeFormat = properties["dateTimeFormat"];
            }
        }

        /// <summary>
        /// Tries parsing <paramref name="stringValue"/> into an enum of the type of <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref name="defaultValue"/> otherwise.</returns>
        protected static object TryParseEnum(Enum defaultValue, string stringValue)
        {
            object result = defaultValue;
            try
            {
                result = Enum.Parse(defaultValue.GetType(), stringValue, true);
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Tries parsing <paramref name="stringValue"/> into a <see cref="bool"/>.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref name="defaultValue"/> otherwise.</returns>
        protected static bool TryParseBoolean(bool defaultValue, string stringValue)
        {
            bool result = defaultValue;
            try
            {
                result = bool.Parse(stringValue);
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Derived factories need to implement this method to create the
        /// actual logger instance.
        /// </summary>
        /// <returns>a new logger instance. Must never be <c>null</c>!</returns>
        protected abstract ILog CreateLogger(string name, LogLevel level, bool showDateTime, bool showLogName, string dateTimeFormat);

        #region ILoggerFactoryAdapter Members

        /// <summary>
        /// Get a ILog instance by <see cref="Type" />.
        /// </summary>
        /// <param name="type">Usually the <see cref="Type" /> of the current class.</param>
        /// <returns>
        /// An ILog instance either obtained from the internal cache or created by a call to <see cref="CreateLogger"/>.
        /// </returns>
        public virtual ILog GetLogger(Type type)
        {
            return GetLoggerInternal(type.FullName);
        }

        /// <summary>
        /// Get a ILog instance by name.
        /// </summary>
        /// <param name="name">Usually a <see cref="Type" />'s Name or FullName property.</param>
        /// <returns>
        /// An ILog instance either obtained from the internal cache or created by a call to <see cref="CreateLogger"/>.
        /// </returns>
        public virtual ILog GetLogger(string name)
        {
            return GetLoggerInternal(name);
        }

        /// <summary>
        /// Get or create a ILog instance by name.
        /// </summary>
        /// <param name="name">Usually a <see cref="Type" />'s Name or FullName property.</param>
        /// <returns>
        /// An ILog instance either obtained from the internal cache or created by a call to <see cref="CreateLogger"/>.
        /// </returns>
        private ILog GetLoggerInternal(string name)
        {
            ILog log = _cachedLoggers[name] as ILog;
            if (log == null)
            {
                lock (_cachedLoggers)
                {
                    log = _cachedLoggers[name] as ILog;
                    if (log == null)
                    {
                        log = CreateLogger(name, _Level, _showLogName, _showDateTime, _dateTimeFormat);
                        if (log == null)
                        {
                            throw new ArgumentException(string.Format("{0} returned null on creating logger instance for name {1}", this.GetType().FullName, name));
                        }
                        _cachedLoggers.Add(name, log);
                    }
                }
            }
            return log;
        }

        #endregion
    }
}
