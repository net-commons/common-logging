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

using System.Collections.Specialized;
using System.Configuration;

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
        /// </summary>
        /// <param name="sectionName">Name of the configuration section.</param>
        /// <returns>
        /// Object created by a corresponding <see cref="IConfigurationSectionHandler"/>.
        /// </returns>
        /// <remarks>
        /// 	<p>
        /// Primary purpose of this method is to allow us to parse and
        /// load configuration sections using the same API regardless
        /// of the .NET framework version.
        /// </p>
        /// </remarks>
        /// <see cref="ConfigurationSectionHandler"/>
        public object GetSection(string sectionName)
        {
            //cannot use ConfigurationManager.GetSection directly due to bug under .NET 4.0
            //  see http://support.microsoft.com/kb/2580188 for more info
            //return ConfigurationManager.GetSection(sectionName);

            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = ((AppSettingsSection)configuration.GetSection(sectionName)).Settings;

            NameValueCollection collection = new NameValueCollection();

            foreach (string setting in settings.AllKeys)
            {
                collection.Set(setting, settings[setting].Value);
            }

            return collection;
        }
    }
}