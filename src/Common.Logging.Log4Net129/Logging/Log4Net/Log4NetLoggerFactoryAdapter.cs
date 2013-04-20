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
using Common.Logging.Factory;
using Common.Logging.Configuration;
using log4net.Config;

namespace Common.Logging.Log4Net
{
    /// <summary>
    /// Concrete subclass of ILoggerFactoryAdapter specific to log4net 1.2.9-1.2.11.
    /// </summary>
    /// <remarks>
    /// The following configuration property values may be configured:
    /// <list type="bullet">
    ///     <item><c>configType</c>: <c>INLINE|FILE|FILE-WATCH|EXTERNAL</c></item>
    ///     <item><c>configFile</c>: log4net configuration file path in case of FILE or FILE-WATCH</item>
    /// </list>
    /// The configType values have the following implications:
    /// <list type="bullet">
    ///     <item>INLINE: simply calls <c>XmlConfigurator.Configure()</c></item>
    ///     <item>FILE: calls <c>XmlConfigurator.Configure(System.IO.FileInfo)</c> using <c>configFile</c>.</item>
    ///     <item>FILE-WATCH: calls <c>XmlConfigurator.ConfigureAndWatch(System.IO.FileInfo)</c> using <c>configFile</c>.</item>
    ///     <item>EXTERNAL: does nothing and expects log4net to be configured elsewhere.</item>
    ///     <item>&lt;any&gt;: calls <c>BasicConfigurator.Configure()</c></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following snippet shows an example of how to configure log4net with Common.Logging:
    /// <code>
    /// &lt;configuration&gt;
    ///   &lt;configSections&gt;
    ///     &lt;sectionGroup name=&quot;common&quot;&gt;
    ///       &lt;section name=&quot;logging&quot;
    ///                type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot;
    ///                requirePermission=&quot;false&quot; /&gt;
    ///     &lt;/sectionGroup&gt;
    ///     &lt;section name=&quot;log4net&quot;
    ///              type=&quot;log4net.Config.Log4NetConfigurationSectionHandler&quot;
    ///              requirePermission=&quot;false&quot; /&gt;
    ///   &lt;/configSections&gt;
    /// 
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;Common.Logging.Log4Net.Log4NetLoggerFactoryAdapter, Common.Logging.Log4Net129&quot;&gt;
    ///         &lt;arg key=&quot;level&quot; value=&quot;ALL&quot; /&gt;
    ///         &lt;arg key=&quot;configType&quot; value=&quot;INLINE&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    /// 
    ///   &lt;log4net debug=&quot;false&quot;&gt;
    /// 
    ///     &lt;appender name=&quot;RollingLogFileAppender&quot; type=&quot;log4net.Appender.RollingFileAppender, log4net&quot;&gt;
    ///       &lt;param name=&quot;File&quot; value=&quot;./Web.log&quot; /&gt;
    ///       &lt;param name=&quot;AppendToFile&quot; value=&quot;true&quot; /&gt;
    ///       &lt;param name=&quot;MaxSizeRollBackups&quot; value=&quot;1&quot; /&gt;
    ///       &lt;param name=&quot;MaximumFileSize&quot; value=&quot;1GB&quot; /&gt;
    ///       &lt;param name=&quot;RollingStyle&quot; value=&quot;Date&quot; /&gt;
    ///       &lt;param name=&quot;StaticLogFileName&quot; value=&quot;false&quot; /&gt;
    /// 
    ///       &lt;layout type=&quot;log4net.Layout.PatternLayout, log4net&quot;&gt;
    ///         &lt;param name=&quot;ConversionPattern&quot; value=&quot;%d [%t] %-5p %c - %m%n&quot; /&gt;
    ///       &lt;/layout&gt;
    /// 
    ///     &lt;/appender&gt;
    /// 
    ///     &lt;appender name=&quot;TraceAppender&quot; type=&quot;log4net.Appender.TraceAppender&quot;&gt;
    ///       &lt;layout type=&quot;log4net.Layout.PatternLayout&quot;&gt;
    ///         &lt;param name=&quot;ConversionPattern&quot; value=&quot;%-5p: %m&quot; /&gt;
    ///       &lt;/layout&gt;
    ///     &lt;/appender&gt;
    /// 
    ///     &lt;root&gt;
    ///       &lt;level value=&quot;ALL&quot; /&gt;
    ///       &lt;appender-ref ref=&quot;TraceAppender&quot; /&gt;
    ///       &lt;appender-ref ref=&quot;RollingLogFileAppender&quot; /&gt;
    ///     &lt;/root&gt;
    /// 
    ///   &lt;/log4net&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// <author>Gilles Bayon</author>
    /// <author>Erich Eichinger</author>
    public class Log4NetLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        /// <summary>
        /// Abstract interface to the underlying log4net runtime
        /// </summary>
        public interface ILog4NetRuntime
        {
            /// <summary>Calls <see cref="XmlConfigurator.Configure()"/></summary>
            void XmlConfiguratorConfigure();
            /// <summary>Calls <see cref="XmlConfigurator.Configure(System.IO.FileInfo)"/></summary>
            void XmlConfiguratorConfigure(string configFile);
            /// <summary>Calls <see cref="XmlConfigurator.ConfigureAndWatch(System.IO.FileInfo)"/></summary>
            void XmlConfiguratorConfigureAndWatch(string configFile);
            /// <summary>Calls <see cref="BasicConfigurator.Configure()"/></summary>
            void BasicConfiguratorConfigure();
            /// <summary>Calls <see cref="LogManager.GetLogger(string)"/></summary>
            log4net.ILog GetLogger(string name);
        }

        private class Log4NetRuntime : ILog4NetRuntime
        {
            public void XmlConfiguratorConfigure()
            {
                XmlConfigurator.Configure();
            }
            public void XmlConfiguratorConfigure(string configFile)
            {
                XmlConfigurator.Configure(new FileInfo(configFile));
            }
            public void XmlConfiguratorConfigureAndWatch(string configFile)
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
            }
            public void BasicConfiguratorConfigure()
            {
                BasicConfigurator.Configure();
            }
            public log4net.ILog GetLogger(string name)
            {
                return log4net.LogManager.GetLogger(name);
            }
        }

        private readonly ILog4NetRuntime _runtime;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="properties">configuration properties, see <see cref="Log4NetLoggerFactoryAdapter"/> for more.</param>
        public Log4NetLoggerFactoryAdapter(NameValueCollection properties)
            : this(properties, new Log4NetRuntime())
        { }

        /// <summary>
        /// Constructor for binary backwards compatibility with non-portableversions
        /// </summary>
        /// <param name="properties">The properties.</param>
        [Obsolete("Use Constructor taking Common.Logging.Configuration.NameValueCollection instead")]
        public Log4NetLoggerFactoryAdapter(System.Collections.Specialized.NameValueCollection properties)
            : this(NameValueCollectionHelper.ToCommonLoggingCollection(properties))
        { }

        /// <summary>
        /// Constructor accepting configuration properties and an arbitrary 
        /// <see cref="ILog4NetRuntime"/> instance.
        /// </summary>
        /// <param name="properties">configuration properties, see <see cref="Log4NetLoggerFactoryAdapter"/> for more.</param>
        /// <param name="runtime">a log4net runtime adapter</param>
        protected Log4NetLoggerFactoryAdapter(NameValueCollection properties, ILog4NetRuntime runtime)
            : base(true)
        {
            if (runtime == null)
            {
                throw new ArgumentNullException("runtime");
            }
            _runtime = runtime;

            // parse config properties
            string configType = ArgUtils.GetValue(properties, "configType", string.Empty).ToUpper();
            string configFile = ArgUtils.GetValue(properties, "configFile", string.Empty);

            // app-relative path?
            if (configFile.StartsWith("~/") || configFile.StartsWith("~\\"))
            {
                configFile = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\'), configFile.Substring(2));
            }

            if (configType == "FILE" || configType == "FILE-WATCH")
            {
                if (configFile == string.Empty)
                {
                    throw new ConfigurationException("Configuration property 'configFile' must be set for log4Net configuration of type 'FILE' or 'FILE-WATCH'.");
                }

                if (!File.Exists(configFile))
                {
                    throw new ConfigurationException("log4net configuration file '" + configFile + "' does not exists");
                }
            }

            switch (configType)
            {
                case "INLINE":
                    _runtime.XmlConfiguratorConfigure();
                    break;
                case "FILE":
                    _runtime.XmlConfiguratorConfigure(configFile);
                    break;
                case "FILE-WATCH":
                    _runtime.XmlConfiguratorConfigureAndWatch(configFile);
                    break;
                case "EXTERNAL":
                    // Log4net will be configured outside of Common.Logging
                    break;
                default:
                    _runtime.BasicConfiguratorConfigure();
                    break;
            }
        }

        /// <summary>
        /// Create a ILog instance by name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override ILog CreateLogger(string name)
        {
            return new Log4NetLogger(_runtime.GetLogger(name));
        }
    }
}
