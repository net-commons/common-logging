#region License

/*
 * Copyright © 2002-2006 the original author or authors.
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
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using Common.Logging.Simple;

namespace Common.Logging
{
	/// <summary>
	/// Used in an application's configuration file (App.Config or Web.Config) to configure the logging subsystem.
	/// </summary>
	/// <remarks>
	/// <example>
	/// An example configuration section that writes log messages to the Console using the built-in Console Logger.
	/// <code lang="XML" escaped="true">
	/// <configuration>
	///		<configSections>
	///			<sectionGroup name="common">
	///				<section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
	///			</sectionGroup>	
	///		</configSections>
	///		<common>
	///			<logging>
	///				<factoryAdapter type="Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging">
	///					<arg key="showLogName" value="true" />
	///					<arg key="showDataTime" value="true" />
	///					<arg key="level" value="ALL" />
	///					<arg key="dateTimeFormat" value="yyyy/MM/dd HH:mm:ss:fff" />
	///				</factoryAdapter>
	///			</logging>
	///		</common>
	/// </configuration>
	/// </code> 
	/// </example>
	/// </remarks>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: ConfigurationSectionHandler.cs,v 1.1 2006/11/13 07:17:55 markpollack Exp $</version>
	public class ConfigurationSectionHandler: IConfigurationSectionHandler
	{

		#region Fields

		private static readonly string LOGFACTORYADAPTER_ELEMENT = "factoryAdapter";
		private static readonly string LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB = "type";
		private static readonly string ARGUMENT_ELEMENT = "arg";
		private static readonly string ARGUMENT_ELEMENT_KEY_ATTRIB = "key";
		private static readonly string ARGUMENT_ELEMENT_VALUE_ATTRIB = "value";

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
		public ConfigurationSectionHandler()
		{
		}

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
		private LogSetting ReadConfiguration( XmlNode section )
		{
			XmlNode logFactoryElement = section.SelectSingleNode( LOGFACTORYADAPTER_ELEMENT );
			
			string factoryTypeString = string.Empty;
			if ( logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB] != null )
				factoryTypeString = logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB].Value;
            
			if ( factoryTypeString == string.Empty )
			{
				throw new ConfigurationException
					( "Required Attribute '" 
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
					factoryType = Type.GetType( factoryTypeString, true, false );
				}
			}
			catch ( Exception e )
			{
				throw new ConfigurationException
					( "Unable to create type '" + factoryTypeString + "'"
					  , e
					);
			}
			
			XmlNodeList propertyNodes = logFactoryElement.SelectNodes( ARGUMENT_ELEMENT );

			NameValueCollection properties = null;
			properties = new NameValueCollection(); // defaults to case-insensitive keys

			foreach ( XmlNode propertyNode in propertyNodes )
			{
				string key = string.Empty;
				string itsValue = string.Empty;

				XmlAttribute keyAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_KEY_ATTRIB];
				XmlAttribute valueAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_VALUE_ATTRIB];

				if ( keyAttrib == null )
				{
					throw new ConfigurationException
						( "Required Attribute '" 
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

				if ( valueAttrib != null )
				{
					itsValue = valueAttrib.Value;
				}

				properties.Add( key, itsValue );
			}

			return new LogSetting( factoryType, properties );
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
		public object Create(object parent, object configContext, XmlNode section)
		{
			int logFactoryElementsCount = section.SelectNodes( LOGFACTORYADAPTER_ELEMENT ).Count;
			
			if ( logFactoryElementsCount > 1 )
			{
                throw new ConfigurationException("Only one <factoryAdapter> element allowed");
			}
			else if ( logFactoryElementsCount == 1 )
			{
				return ReadConfiguration( section );
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}

