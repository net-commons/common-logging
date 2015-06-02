#region License

/*
 * Copyright © 2002-2015 the original author or authors.
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

namespace Common.Logging.ApplicationInsights
{
    using Common.Logging.Configuration;
    using System;

    /// <summary>
    /// Concrete subclass of ILoggerFactoryAdapter specific to ApplicationInsights.
    /// </summary>
    /// <remarks>
    /// The following configuration property values must be configured:
    /// <list type="bullet">
    ///     <item><c>InstrumentationKey</c>: <c>Application Insights InstrumentationKey</c></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following snippet shows how to configure ApplicationInsights logging for Common.Logging:
    /// <code>
    /// &lt;configuration&gt;
    ///   &lt;configSections&gt;
    ///       &lt;section name=&quot;logging&quot; type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot; /&gt;
    ///   &lt;/configSections&gt;
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;Common.Logging.ApplicationInsights.ApplicationInsightsLoggerFactoryAdapter, Common.Logging.ApplicationInsights&quot;&gt;
    ///         &lt;arg key=&quot;InstrumentationKey&quot; value=&quot;[InstrumentationKey]&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// <author>Mihail Smacinih</author>
    public class ApplicationInsightsLoggerFactoryAdapter : Simple.AbstractSimpleLoggerFactoryAdapter
    {
        private readonly string instrumentationKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsLoggerFactoryAdapter"/> class.
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
        public ApplicationInsightsLoggerFactoryAdapter(NameValueCollection properties)
            : base(properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties argument is null");
            }

            if (properties["InstrumentationKey"] == null)
            {
                throw new ArgumentException("InstrumentationKey property is not specified");
            }

            this.instrumentationKey = properties["InstrumentationKey"];
        }

        /// <summary>
        /// Creates a new <see cref="ILog"/> instance.
        /// </summary>
        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            ILog log = new ApplicationInsightsLogger(instrumentationKey, name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
            return log;
        }
    }
}
