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
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Common.Logging.Configuration;

namespace Common.Logging.MultipleLogger
{
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var mainConfigSectionHandler = new Common.Logging.ConfigurationSectionHandler();
            var logSettings = new List<LogSetting>();
            var adapterRegistrations = section.SelectNodes("factoryAdapter");


            //leverage the 'base' config section handler to process ea. individual factoryAdapter listing for us
            foreach (var adapterRegistration in adapterRegistrations)
            {
                var adapterRegistrationNode = adapterRegistration as XmlNode;

                if (null != adapterRegistrationNode)
                {
                    var nodeToParse = WrapAdapterRegistrationNodeInParentNode(adapterRegistrationNode);
                    logSettings.Add(mainConfigSectionHandler.Create(parent as LogSetting, configContext, nodeToParse));
                }
            }

            return logSettings;
        }

        /// <summary>
        /// Wraps the adapter node in parent node.
        /// </summary>
        /// <param name="adapterRegistrationNode">The adapter registration node.</param>
        /// <returns>XmlNode.</returns>
        /// <remarks>the 'base' config section handler expects the factoryAdapter node to *not* be the outer-most Xml parent node
        /// so in order to properly leverage the main config handler for this task we first have to 'wrap' each factoryAdapter entry
        /// in an outer XmlNode before it can be used as input to the main handler to produce an adapter.</remarks>
        private XmlNode WrapAdapterRegistrationNodeInParentNode(XmlNode adapterRegistrationNode)
        {
            var xmlDoc = new XmlDocument();
            var parentNode = xmlDoc.CreateNode(XmlNodeType.Element, "placeholderParent", string.Empty);
            parentNode.AppendChild(xmlDoc.ImportNode(adapterRegistrationNode, true));
            return parentNode;
        }
    }
}