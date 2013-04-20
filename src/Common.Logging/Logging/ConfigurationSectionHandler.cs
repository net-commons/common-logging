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
using System.Collections;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Xml;
using Common.Logging.Simple;
using Common.Logging.Configuration;

namespace Common.Logging
{
    /// <summary>
    /// Used in an application's configuration file (App.Config or Web.Config) to configure the logging subsystem.
    /// </summary>
    /// <example>
    /// An example configuration section that writes log messages to the Console using the
    /// built-in Console Logger.
    /// <code lang="XML">
    /// &lt;configuration&gt;
    ///     &lt;configSections&gt;
    ///       &lt;sectionGroup name=&quot;common&quot;&gt;
    ///         &lt;section name=&quot;logging&quot; type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot; /&gt;
    ///       &lt;/sectionGroup&gt; 
    ///     &lt;/configSections&gt;
    ///     &lt;common&gt;
    ///       &lt;logging&gt;
    ///         &lt;factoryAdapter type=&quot;Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging&quot;&gt;
    ///           &lt;arg key=&quot;showLogName&quot; value=&quot;true&quot; /&gt;
    ///           &lt;arg key=&quot;showDataTime&quot; value=&quot;true&quot; /&gt;
    ///           &lt;arg key=&quot;level&quot; value=&quot;ALL&quot; /&gt;
    ///           &lt;arg key=&quot;dateTimeFormat&quot; value=&quot;yyyy/MM/dd HH:mm:ss:fff&quot; /&gt;
    ///         &lt;/factoryAdapter&gt;
    ///       &lt;/logging&gt;
    ///     &lt;/common&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {

        #region Fields

        private static readonly string LOGFACTORYADAPTER_ELEMENT = "factoryAdapter";
        private static readonly string LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB = "type";
        private static readonly string ARGUMENT_ELEMENT = "arg";
        private static readonly string ARGUMENT_ELEMENT_KEY_ATTRIB = "key";
        private static readonly string ARGUMENT_ELEMENT_VALUE_ATTRIB = "value";

        #endregion

        /// <summary>
        /// Ensure static fields get initialized before any class member 
        /// can be accessed (avoids beforeFieldInit)
        /// </summary>
        static ConfigurationSectionHandler()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationSectionHandler()
        { }

        /// <summary>
        /// Retrieves the <see cref="Type" /> of the logger the use by looking at the logFactoryAdapter element
        /// of the logging configuration element.
        /// </summary>
        /// <param name="section"></param>
        /// <returns>
        /// A <see cref="LogSetting" /> object containing the specified type that implements 
        /// <see cref="ILoggerFactoryAdapter" /> along with zero or more properties that will be 
        /// passed to the logger factory adapter's constructor as an <see cref="IDictionary" />.
        /// </returns>
        private LogSetting ReadConfiguration(XmlNode section)
        {
            XmlNode logFactoryElement = section.SelectSingleNode(LOGFACTORYADAPTER_ELEMENT);

            string factoryTypeString = string.Empty;
            if (logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB] != null)
                factoryTypeString = logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB].Value;

            if (factoryTypeString == string.Empty)
            {
                throw new ConfigurationException
                  ("Required Attribute '"
                  + LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB
                  + "' not found in element '"
                  + LOGFACTORYADAPTER_ELEMENT
                  + "'"
                  );
            }

            Type factoryType = null;
            try
            {
                if (String.Compare(factoryTypeString, "CONSOLE", true) == 0)
                {
                    factoryType = typeof(ConsoleOutLoggerFactoryAdapter);
                }
                else if (String.Compare(factoryTypeString, "TRACE", true) == 0)
                {
                    factoryType = typeof(TraceLoggerFactoryAdapter);
                }
                else if (String.Compare(factoryTypeString, "NOOP", true) == 0)
                {
                    factoryType = typeof(NoOpLoggerFactoryAdapter);
                }
                else
                {
                    factoryType = Type.GetType(factoryTypeString, true, false);
                }
            }
            catch (Exception e)
            {
                throw new ConfigurationException
                  ("Unable to create type '" + factoryTypeString + "'"
                    , e
                  );
            }

            XmlNodeList propertyNodes = logFactoryElement.SelectNodes(ARGUMENT_ELEMENT);

            NameValueCollection properties = null;
            properties = new NameValueCollection(); // defaults to case-insensitive keys

            foreach (XmlNode propertyNode in propertyNodes)
            {
                string key = string.Empty;
                string itsValue = string.Empty;

                XmlAttribute keyAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_KEY_ATTRIB];
                XmlAttribute valueAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_VALUE_ATTRIB];

                if (keyAttrib == null)
                {
                    throw new ConfigurationException
                      ("Required Attribute '"
                        + ARGUMENT_ELEMENT_KEY_ATTRIB
                        + "' not found in element '"
                        + ARGUMENT_ELEMENT
                        + "'"
                      );
                }
                else
                {
                    key = keyAttrib.Value;
                }

                if (valueAttrib != null)
                {
                    itsValue = valueAttrib.Value;
                }

                properties.Add(key, itsValue);
            }

            return new LogSetting(factoryType, properties);
        }

        /// <summary>
        /// Verifies that the logFactoryAdapter element appears once in the configuration section.
        /// </summary>
        /// <param name="parent">settings of a parent section - atm this must always be null</param>
        /// <param name="configContext">Additional information about the configuration process.</param>
        /// <param name="section">The configuration section to apply an XPath query too.</param>
        /// <returns>
        /// A <see cref="LogSetting" /> object containing the specified logFactoryAdapter type
        /// along with user supplied configuration properties.
        /// </returns>
        public LogSetting Create(LogSetting parent, object configContext, XmlNode section)
        {
            if (parent != null)
            {
                throw new ConfigurationException("parent configuration sections are not allowed");
            }

            int logFactoryElementsCount = section.SelectNodes(LOGFACTORYADAPTER_ELEMENT).Count;

            if (logFactoryElementsCount > 1)
            {
                throw new ConfigurationException("Only one <factoryAdapter> element allowed");
            }
            else if (logFactoryElementsCount == 1)
            {
                return ReadConfiguration(section);
            }
            else
            {
                return null;
            }
        }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Verifies that the logFactoryAdapter element appears once in the configuration section.
        /// </summary>
        /// <param name="parent">The parent of the current item.</param>
        /// <param name="configContext">Additional information about the configuration process.</param>
        /// <param name="section">The configuration section to apply an XPath query too.</param>
        /// <returns>
        /// A <see cref="LogSetting" /> object containing the specified logFactoryAdapter type
        /// along with user supplied configuration properties.
        /// </returns>
        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            return Create(parent as LogSetting, configContext, section);
        }

        #endregion
    }
}
