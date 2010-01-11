#region License

/*
 * Copyright 2002-2009 the original author or authors.
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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="CommonLoggingEntlibTraceListener"/>.
    /// </summary>
    [Assembler(typeof(CommonLoggingEntlibTraceListenerAssembler))]
    public class CommonLoggingEntlibTraceListenerData : TraceListenerData
    {
        private const string loggerNameFormatProperty = "loggerNameFormat";

        /// <summary>
        /// The logger name format to use for creating logger names from <see cref="NamedConfigurationElement.Name"/> and 
        /// </summary>
        [ConfigurationProperty(loggerNameFormatProperty, IsRequired = false)]
        public string LoggerNameFormat
        {
            get { return (string) base[loggerNameFormatProperty ] ; }
            set { base[loggerNameFormatProperty] = value; }
        }

        /// <summary>
        /// Gets or Sets the formatter name
        /// </summary>
        [ConfigurationProperty("formatter", IsRequired = false)]
        public string Formatter
        {
            get
            {
                return (string)base["formatter"];
            }
            set
            {
                base["formatter"] = value;
            }
        }
    }
}