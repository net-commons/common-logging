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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder;

namespace Common.Logging.EntLib
{
    ///<summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code. 
    /// Represents the process to build a <see cref="CommonLoggingEntlibTraceListener"/> described by a 
    /// <see cref="CommonLoggingEntlibTraceListenerData"/> configuration object. 
    ///</summary>
    public class CommonLoggingEntlibTraceListenerAssembler : TraceListenerAsssembler
    {
        ///<summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code. 
        /// Builds a <see cref="CommonLoggingEntlibTraceListener"/> described by a 
        /// <see cref="CommonLoggingEntlibTraceListenerData"/> configuration object. 
        ///</summary>
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            CommonLoggingEntlibTraceListenerData data = (CommonLoggingEntlibTraceListenerData)objectConfiguration;
            ILogFormatter formatter = base.GetFormatter(context, data.Formatter, configurationSource, reflectionCache);

            CommonLoggingEntlibTraceListener listener = (CommonLoggingEntlibTraceListener)System.Activator.CreateInstance(objectConfiguration.Type, new object[] { objectConfiguration, formatter });
            return listener;
        }
    }
}