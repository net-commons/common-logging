#region License

/*
 * Copyright © 2002-2007 the original author or authors.
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

#region Imports

using System;
using System.IO;
using System.Collections.Specialized;

using LogManagerNLog = NLog.LogManager;
using XmlLoggingConfigurationNLog = NLog.Config.XmlLoggingConfiguration;

#endregion 

namespace Common.Logging.NLog
{
	/// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to NLog.
	/// </summary>
	/// <author>Bruno Baia</author>
    /// <version>$Id: $</version>
    public class NLogLoggerFactoryAdapter : ILoggerFactoryAdapter
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="properties"></param>
        public NLogLoggerFactoryAdapter(NameValueCollection properties)
		{
			string configType = string.Empty;
			
			if ( properties["configType"] != null )
			{
				configType = properties["configType"].ToUpper();	
			}

			string configFile = string.Empty;
			if ( properties["configFile"] != null )
			{
				configFile = properties["configFile"];			
				if (configFile.StartsWith("~/") || configFile.StartsWith("~\\"))
				{
					configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\') + "/", configFile.Substring(2));
				}
			}

			if ( configType == "FILE" || configType == "FILE-WATCH" )
			{
                if (configFile == string.Empty)
                {
                    throw new ConfigurationException("Configration property 'configFile' must be set for NLog configuration of type 'FILE'.");
                }

                if (!File.Exists(configFile))
                {
                    throw new ConfigurationException("NLog configuration file '" + configFile + "' does not exists");
                }
			}

			switch ( configType )
			{
				case "INLINE":
					break;
				case "FILE":
                    LogManagerNLog.Configuration = new XmlLoggingConfigurationNLog(configFile);
					break;
				default:
					break;
			}
		}

		#region ILoggerFactoryAdapter Members

		/// <summary>
		/// Get a ILog instance by type name 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ILog GetLogger(string name)
		{
            return new NLogLogger(LogManagerNLog.GetLogger(name));
		}

		/// <summary>
		/// Get a ILog instance by type 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ILog GetLogger(Type type)
		{
            return new NLogLogger(LogManagerNLog.GetLogger(type.FullName));
		}

		#endregion
	}
}
