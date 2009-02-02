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
using System.Collections.Specialized;

namespace Common.Logging.Simple
{
	/// <summary>
	/// Factory for creating <see cref="ILog" /> instances that write data to <see cref="Console.Out" />.
	/// </summary>
	/// <seealso cref="AbstractSimpleLoggerFactoryAdapter"/>
	/// <seealso cref="LogManager.Adapter"/>
	/// <seealso cref="ConfigurationSectionHandler"/>
    /// <author>Gilles Bayon</author>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
	public class ConsoleOutLoggerFactoryAdapter: AbstractSimpleLoggerFactoryAdapter 
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOutLoggerFactoryAdapter"/> class using default 
        /// settings.
        /// </summary>
	    public ConsoleOutLoggerFactoryAdapter() : base(null)            
	    {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOutLoggerFactoryAdapter"/> class.
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
		public ConsoleOutLoggerFactoryAdapter(NameValueCollection properties):base(properties)
		{}

	    /// <summary>
	    /// Creates a new <see cref="ConsoleOutLogger"/> instance.
	    /// </summary>
	    protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
	    {
            ILog log = new ConsoleOutLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
	        return log;	        
	    }
	}
}
