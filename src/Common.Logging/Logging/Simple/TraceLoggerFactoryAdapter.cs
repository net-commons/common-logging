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

//using System.Collections.Specialized;
using System.Diagnostics;
using Common.Logging.Configuration;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Factory for creating <see cref="ILog" /> instances that send 
    /// everything to the <see cref="System.Diagnostics.Trace"/> output stream.
    /// </summary>
    /// <remarks>
    /// Beware not to use <see cref="CommonLoggingTraceListener"/> in combination with this logger factory
    /// as this would result in an endless loop for obvious reasons!
    /// <example>
    /// Below is an example how to configure this adapter:
    /// <code>
    /// &lt;configuration&gt;
    /// 
    ///   &lt;configSections&gt;
    ///     &lt;sectionGroup name=&quot;common&quot;&gt;
    ///       &lt;section name=&quot;logging&quot;
    ///                type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot;
    ///                requirePermission=&quot;false&quot; /&gt;
    ///     &lt;/sectionGroup&gt;
    ///   &lt;/configSections&gt;
    /// 
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;Common.Logging.Simple.TraceLoggerFactoryAdapter, Common.Logging&quot;&gt;
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
    /// <seealso cref="LogManager.Adapter"/>
    /// <seealso cref="ConfigurationSectionHandler"/>
    /// <author>Gilles Bayon</author>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    public class TraceLoggerFactoryAdapter : Simple.AbstractSimpleLoggerFactoryAdapter
    {
        private bool _useTraceSource = false;

        /// <summary>
        /// Whether to use <see cref="Trace"/>.<c>TraceXXXX(string,object[])</c> methods for logging
        /// or <see cref="TraceSource"/>.
        /// </summary>
        [CoverageExclude]
        public bool UseTraceSource
        {
            get { return _useTraceSource; }
            set { _useTraceSource = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLoggerFactoryAdapter"/> class using default settings.
        /// </summary>
        public TraceLoggerFactoryAdapter()
            : base(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLoggerFactoryAdapter"/> class.
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
        public TraceLoggerFactoryAdapter(NameValueCollection properties)
            : base(properties)
        {
            _useTraceSource = ArgUtils.TryParse(false, properties["useTraceSource"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSimpleLoggerFactoryAdapter"/> class with 
        /// default settings for the loggers created by this factory.
        /// </summary>
        public TraceLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel, string dateTimeFormat, bool useTraceSource) 
            : base(level, showDateTime, showLogName, showLevel, dateTimeFormat)
        {
            _useTraceSource = useTraceSource;
        }

        /// <summary>
        /// Creates a new <see cref="TraceLogger"/> instance.
        /// </summary>
        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            ILog log = new TraceLogger(_useTraceSource, name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
            return log;
        }
    }
}
