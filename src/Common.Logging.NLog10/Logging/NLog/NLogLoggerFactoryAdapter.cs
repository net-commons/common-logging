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
using System.IO;
using Common.Logging.Configuration;
using Common.Logging.Factory;

namespace Common.Logging.NLog
{
    /// <summary>
    /// Concrete subclass of ILoggerFactoryAdapter specific to NLog 1.0.0.505.
    /// </summary>
    /// <remarks>
    /// <para>Note, that you cannot use NLog in medium trust environments unless you use an unsigned build</para>
    /// The following configuration property values may be configured:
    /// <list type="bullet">
    ///     <item><c>configType</c>: <c>INLINE|FILE</c></item>
    ///     <item><c>configFile</c>: NLog XML configuration file path in case of FILE</item>
    /// </list>
    /// The configType values have the following implications:
    /// <list type="bullet">
    ///     <item>FILE: calls <c>NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(configFile)</c>.</item>
    ///     <item>&lt;any other value&gt;: expects NLog to be configured externally</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following snippet shows how to configure EntLib logging for Common.Logging:
    /// <code>
    /// &lt;configuration&gt;
    ///   &lt;configSections&gt;
    ///       &lt;section name=&quot;logging&quot; type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot; /&gt;
    ///   &lt;/configSections&gt;
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;Common.Logging.NLog.NLogLoggerFactoryAdapter, Common.Logging.NLog&quot;&gt;
    ///         &lt;arg key=&quot;configType&quot; value=&quot;FILE&quot; /&gt;
    ///         &lt;arg key=&quot;configFile&quot; value=&quot;~/nlog.config&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// <author>Bruno Baia</author>
    /// <author>Erich Eichinger</author>
    public class NLogLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
         /// <summary>
        /// Constructor for binary backwards compatibility with non-portableversions
        /// </summary>
        /// <param name="properties">The properties.</param>
        [Obsolete("Use Constructor taking Common.Logging.Configuration.NameValueCollection instead")]
        public NLogLoggerFactoryAdapter(System.Collections.Specialized.NameValueCollection properties)
            : this(NameValueCollectionHelper.ToCommonLoggingCollection(properties))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="properties"></param>
        public NLogLoggerFactoryAdapter(NameValueCollection properties)
            : base(true)
        {
            string configType = string.Empty;
            string configFile = string.Empty;

            if (properties != null) {
                if (properties["configType"] != null) {
                    configType = properties["configType"].ToUpper();
                }

                if (properties["configFile"] != null) {
                    configFile = properties["configFile"];
                    if (configFile.StartsWith("~/") || configFile.StartsWith("~\\")) {
                        configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\') + "/", configFile.Substring(2));
                    }
                }

                if (configType == "FILE") {
                    if (configFile == string.Empty) {
                        throw new ConfigurationException("Configuration property 'configFile' must be set for NLog configuration of type 'FILE'.");
                    }

                    if (!File.Exists(configFile)) {
                        throw new ConfigurationException("NLog configuration file '" + configFile + "' does not exists");
                    }
                }
            }
            switch (configType) {
                case "INLINE":
                    break;
                case "FILE":
                    global::NLog.LogManager.Configuration = new global::NLog.Config.XmlLoggingConfiguration(configFile);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get a ILog instance by type name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override ILog CreateLogger(string name)
        {
            return new NLogLogger(global::NLog.LogManager.GetLogger(name));
        }
    }
}
