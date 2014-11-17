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
using Common.Logging.Configuration;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Factory for creating <see cref="ILog" /> instances that write data using <see cref="System.Diagnostics.Debug.WriteLine(string)" />.
    /// </summary>
    /// <remarks>
    /// <example>
    /// Below is an example how to configure this adapter:
    /// <code>
    /// &lt;configuration&gt;
    /// 
    ///   &lt;configSections&gt;
    ///     &lt;sectionGroup key=&quot;common&quot;&gt;
    ///       &lt;section key=&quot;logging&quot;
    ///                type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot;
    ///                requirePermission=&quot;false&quot; /&gt;
    ///     &lt;/sectionGroup&gt;
    ///   &lt;/configSections&gt;
    /// 
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;Common.Logging.Simple.DebugLoggerFactoryAdapter, Common.Logging&quot;&gt;
    ///         &lt;arg key=&quot;level&quot; value=&quot;ALL&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    /// 
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// </remarks>
    /// <seealso cref="AbstractSimpleLoggerFactoryAdapter"/>
    /// <author>Gilles Bayon</author>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    public class DebugLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLoggerFactoryAdapter"/> class using default 
        /// settings.
        /// </summary>
        public DebugLoggerFactoryAdapter()
            : base(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>
        /// Looks for level, showDateTime, showLogName, dateTimeFormat items from 
        /// <paramref key="properties" /> for use when the GetLogger methods are called.
        /// <see cref="System.Configuration.ConfigurationManager"/> for more information on how to use the 
        /// standard .NET application configuraiton file (App.config/Web.config) 
        /// to configure this adapter.
        /// </remarks>
        /// <param name="properties">The key value collection, typically specified by the user in 
        /// a configuration section named common/logging.</param>
        public DebugLoggerFactoryAdapter(NameValueCollection properties)
            : base(properties)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSimpleLoggerFactoryAdapter"/> class with 
        /// default settings for the loggers created by this factory.
        /// </summary>
        public DebugLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel, string dateTimeFormat) 
            : base(level, showDateTime, showLogName, showLevel, dateTimeFormat)
        { }

        /// <summary>
        /// Creates a new <see cref="DebugOutLogger"/> instance.
        /// </summary>
        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {            
            ILog log = new DebugOutLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
            return log;
        }
    }
}
