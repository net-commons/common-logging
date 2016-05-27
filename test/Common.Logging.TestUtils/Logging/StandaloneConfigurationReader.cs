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

using System.Configuration;
using System.Xml;

namespace Common.Logging
{
    /// <summary>
    /// A ConfigurationReader implementation that call the ConfigurationSectionHandler on a 
    /// supplied XML string.
    /// </summary>
    /// <author>Mark Pollack</author>
    public class StandaloneConfigurationReader : IConfigurationReader
    {
        private string xmlString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneConfigurationReader"/> class.
        /// </summary>
        public StandaloneConfigurationReader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneConfigurationReader"/> class.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        public StandaloneConfigurationReader(string xmlString)
        {
            XmlString = xmlString;
        }

        /// <summary>
        /// Gets or sets the XML string.
        /// </summary>
        /// <value>The XML string.</value>
        public string XmlString
        {
            get { return xmlString; }
            set { xmlString = value; }
        }

        /// <summary>
        /// Parses the configuration section and returns the resulting object.
        /// </summary>
        /// <param name="sectionName">Name of the configuration section.</param>
        /// <returns>
        /// Object created by a corresponding <see cref="System.Configuration.IConfigurationSectionHandler"/>.
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
            ConfigurationSectionHandler handler = new ConfigurationSectionHandler();
            return handler.Create(null, null, BuildConfigurationSection(XmlString));
        }

        /// <summary>
        /// Builds the configuration section XmlNode given an Xml string.
        /// </summary>
        /// <param name="xml">The XML string</param>
        /// <returns>The XmlNode of the document corresponding to the string.</returns>
        private static XmlNode BuildConfigurationSection(string xml)
        {
            ConfigXmlDocument doc = new ConfigXmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}