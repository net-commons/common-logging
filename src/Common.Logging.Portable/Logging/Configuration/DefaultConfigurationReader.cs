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
namespace Common.Logging.Configuration
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationReader"/> that uses the standard .NET 
    /// configuration APIs, ConfigurationSettings in 1.x and ConfigurationManager in 2.0
    /// </summary>
    /// <author>Mark Pollack</author>
    public class DefaultConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// Parses the configuration section and returns the resulting object.
        /// Using the <c>System.Configuration.ConfigurationManager</c>
        /// </summary>
        /// <param key="sectionName">Name of the configuration section.</param>
        /// <returns>
        /// Object created by a corresponding <c>IConfigurationSectionHandler"</c>
        /// </returns>
        /// <remarks>
        /// 	<p>
        /// Primary purpose of this method is to allow us to parse and
        /// load configuration sections using the same API regardless
        /// of the .NET framework version.
        /// </p>
        /// </remarks>
        public object GetSection(string sectionName)
        {
#if PORTABLE
            // We should instead look for something implementing 
            // IConfigurationReader in (platform specific) Common.Logging dll and use that
            const string configManager40 = "System.Configuration.ConfigurationManager, System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            var configurationManager = Type.GetType(configManager40);
            if(configurationManager == null)
            {
                // Silverlight, and maybe if System.Configuration is not loaded?
                return null;
            }
            var getSection = configurationManager.GetMethod("GetSection", new[] { typeof(string) });
            if (getSection == null)
                throw new PlatformNotSupportedException("Could not find System.Configuration.ConfigurationManager.GetSection method");

            return getSection.Invoke(null, new[] {sectionName});
#else
            return System.Configuration.ConfigurationManager.GetSection(sectionName);
#endif
        }
    }
}