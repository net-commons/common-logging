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
using MonoTouch.Foundation;
using Common.Logging.Simple;
using System.Collections.Specialized;

namespace Common.Logging.Configuration
{
    /// <summary>
    /// MonoTouch implementation of configuration reader.  Each section corresponds to a plist on the device.
    /// </summary>
    /// <author>adamnation</author>
    public class DefaultConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// Parses the configuration section and returns the resulting object.
        /// </summary>
        /// <param name="sectionName">Path to the plist on the device</param>
        /// <returns>
        /// NSDictionary of the settings in the plist.
        /// </returns>
        /// <remarks>
        /// <p>
        /// Primary purpose of this method is to allow us to load settings from a plist on the device.
        /// </p>
        /// </remarks>
        /// <see cref="ConfigurationSectionHandler"/>
        public object GetSection (string sectionName)
		{	
			NameValueCollection properties = new NameValueCollection();
			var nsDict = new NSDictionary (NSBundle.MainBundle.PathForResource (sectionName, null));
			foreach (var key in nsDict.Keys) 
			{
				properties.Add(key.ToString(), nsDict[key].ToString());
			}

			return new ConsoleOutLoggerFactoryAdapter(properties);
        }
    }
}