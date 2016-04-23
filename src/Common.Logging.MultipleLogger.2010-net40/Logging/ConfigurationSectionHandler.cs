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