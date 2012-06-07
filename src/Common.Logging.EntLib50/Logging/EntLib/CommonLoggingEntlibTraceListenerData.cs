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

using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="CommonLoggingEntlibTraceListener"/>.
    /// </summary>
    public class CommonLoggingEntlibTraceListenerData : TraceListenerData
    {
        private const string loggerNameFormatProperty = "loggerNameFormat";

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingEntlibTraceListenerData"/> class.
        /// </summary>
        public CommonLoggingEntlibTraceListenerData() : base(typeof(CommonLoggingEntlibTraceListener))
        {
            ListenerDataType = typeof(CommonLoggingEntlibTraceListenerData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingEntlibTraceListenerData"/> class.
        /// </summary>
        /// <param name="loggerNameFormat">The logger name format.</param>
        /// <param name="formatterName">Name of the formatter.</param>
        public CommonLoggingEntlibTraceListenerData(string loggerNameFormat, string formatterName)
            : this("unnamed", loggerNameFormat, formatterName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingEntlibTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="loggerNameFormat">The logger name format.</param>
        /// <param name="formatterName">Name of the formatter.</param>
        public CommonLoggingEntlibTraceListenerData(string name, string loggerNameFormat, string formatterName)
            : this(name, typeof(CommonLoggingEntlibTraceListener), loggerNameFormat, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingEntlibTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="loggerNameFormat">The logger name format.</param>
        /// <param name="formatterName">Name of the formatter.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
        public CommonLoggingEntlibTraceListenerData(string name, string loggerNameFormat, string formatterName, TraceOptions traceOutputOptions)
            : this(name, typeof(CommonLoggingEntlibTraceListener), loggerNameFormat, formatterName, traceOutputOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingEntlibTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="traceListenerType">Type of the trace listener.</param>
        /// <param name="loggerNameFormat">The logger name format.</param>
        /// <param name="formatterName">Name of the formatter.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
        public CommonLoggingEntlibTraceListenerData(string name, Type traceListenerType, string loggerNameFormat, string formatterName, TraceOptions traceOutputOptions)
            : base(name, traceListenerType, traceOutputOptions)
        {
            LoggerNameFormat = loggerNameFormat;
            Formatter = formatterName;
        }

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
            get { return (string)base["formatter"]; }
            set { base["formatter"] = value; }
        }

        /// <summary>
        /// Gets the creation expression used to produce a <see cref="T:Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.TypeRegistration"/> during
        /// <see cref="M:Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData.GetRegistrations"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Linq.Expressions.Expression"/> that creates a <see cref="T:System.Diagnostics.TraceListener"/>
        /// </returns>
        /// <remarks>
        /// This must be overridden by a subclass, but is not marked as abstract due to configuration serialization needs.
        /// </remarks>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () => new CommonLoggingEntlibTraceListener(this, Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter));
        }
    }
}