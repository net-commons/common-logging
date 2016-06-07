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
using Serilog;

namespace Common.Logging.Serilog
{
    /// <summary>
    /// Routes log events to Common.Logging infrastructure.
    /// </summary>
    /// <example>
    /// To route all events logged using serilog to Common.Logging, you need to add the folling appsettings
    /// <code>
    /// &lt;add key=&quot;serilog:using&quot; value=&quot;Common.Logging.Serilog1514&quot; /&gt;
    /// &lt;add key=&quot;serilog:write-to:CommonLogging&quot; /&gt;
    /// </code>
    /// </example>
    /// <author>Erich Eichinger</author>
    public class SerilogLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {

        /// <summary>
        /// Constructor for binary backwards compatibility with non-portable versions
        /// </summary>
        /// <param name="properties">The properties.</param>
        [Obsolete("Use Constructor taking Common.Logging.Configuration.NameValueCollection instead")]
        public SerilogLoggerFactoryAdapter(System.Collections.Specialized.NameValueCollection properties)
            : this(NameValueCollectionHelper.ToCommonLoggingCollection(properties))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="properties"></param>
        public SerilogLoggerFactoryAdapter(NameValueCollection properties)
            : base(true)
        {
            
        }

        /// <summary>
        /// Get a ILog instance by type name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override ILog CreateLogger(string name)
        {
            return new SerilogLogger(new LoggerConfiguration().ReadFrom.AppSettings().Enrich.WithProperty("Common.Logging.Type", name).CreateLogger());
        }
    }
}
