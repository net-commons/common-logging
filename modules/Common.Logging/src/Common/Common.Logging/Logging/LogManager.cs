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
using System.Configuration;
using Common.Logging.Simple;

namespace Common.Logging
{
	/// <summary>
	/// The LogManager can produce ILogFactory for various logging APIs,
	/// most notably for log4net. 
	/// Other implemenations such as
	/// * SimpleLogger
	/// * NoOpLogger are also supported.
	/// </summary>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: LogManager.cs,v 1.1 2006/11/13 07:17:55 markpollack Exp $</version>
	public sealed class LogManager
	{
		private static ILoggerFactoryAdapter _adapter = null;
		private static object _loadLock = new object();
		private static readonly string COMMON_SECTION_LOGGING = "common/logging";

		/// <summary>
		/// Initializes a new instance of the <see cref="LogManager" /> class. 
		/// </summary>
		/// <remarks>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </remarks>
		private LogManager()
		{ }


        /// <summary>
        /// Gets or sets the adapter.
        /// </summary>
        /// <value>The adapter.</value>
        public static ILoggerFactoryAdapter Adapter
		{
			get
			{
				if ( _adapter == null )
				{
					lock (_loadLock)
					{
						if (_adapter == null)
						{	
							_adapter = BuildLoggerFactoryAdapter();
						}
					}
				}
				return _adapter;				
			}
		    set
			{
				lock (_loadLock)
				{
					_adapter = value;
				}
			}

		}


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
		public static ILog GetLogger( Type type )
		{
			return Adapter.GetLogger( type );
		}


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
		public static ILog GetLogger( string name )
		{
			return Adapter.GetLogger(name);
		}


        /// <summary>
        /// Builds the logger factory adapter.
        /// </summary>
        /// <returns></returns>
		private static ILoggerFactoryAdapter BuildLoggerFactoryAdapter()
		{
			LogSetting setting = null;
			try
			{
                setting = (LogSetting)GetSection(COMMON_SECTION_LOGGING);
            }
			catch ( Exception ex )
			{
				ILoggerFactoryAdapter defaultFactory = BuildDefaultLoggerFactoryAdapter();
				ILog log = defaultFactory.GetLogger( typeof(LogManager) );
				log.Warn( "Unable to read configuration. Using default logger.", ex );
				return defaultFactory;
			}

			if ( setting!= null && !typeof ( ILoggerFactoryAdapter ).IsAssignableFrom( setting.FactoryAdapterType ) )
			{
				ILoggerFactoryAdapter defaultFactory = BuildDefaultLoggerFactoryAdapter();
				ILog log = defaultFactory.GetLogger( typeof(LogManager) );
                log.Warn("Type " + setting.FactoryAdapterType.FullName + " does not implement ILoggerFactoryAdapter. Using default logger");
				return defaultFactory;
			}

			ILoggerFactoryAdapter instance = null;

			if (setting!=null)
			{
				if (setting.Properties.Count>0)
				{
					try
					{
						object[] args = {setting.Properties};

						instance = (ILoggerFactoryAdapter)Activator.CreateInstance( setting.FactoryAdapterType, args );
					}
					catch ( Exception ex )
					{
						ILoggerFactoryAdapter defaultFactory = BuildDefaultLoggerFactoryAdapter();
						ILog log = defaultFactory.GetLogger( typeof(LogManager) );
						log.Warn( "Unable to create instance of type " + setting.FactoryAdapterType.FullName + ". Using default logger.", ex );
						return defaultFactory;
					}					
				}
				else
				{
					try
					{
						instance = (ILoggerFactoryAdapter)Activator.CreateInstance( setting.FactoryAdapterType );
					}
					catch ( Exception ex )
					{
						ILoggerFactoryAdapter defaultFactory = BuildDefaultLoggerFactoryAdapter();
						ILog log = defaultFactory.GetLogger( typeof(LogManager) );
						log.Warn( "Unable to create instance of type " + setting.FactoryAdapterType.FullName + ". Using default logger.", ex );
						return defaultFactory;
					}
				}
			}
			else
			{
				ILoggerFactoryAdapter defaultFactory = BuildDefaultLoggerFactoryAdapter();
				ILog log = defaultFactory.GetLogger( typeof(LogManager) );
				log.Warn( "Unable to read configuration common/logging. Using default logger (ConsoleLogger)." );
				return defaultFactory;
			}

			return instance;
		}

        private static object GetSection(string sectionName)
        {
#if !NET_2_0
            return ConfigurationSettings.GetConfig(sectionName);
#else
            return ConfigurationManager.GetSection(sectionName);
#endif
        }


        /// <summary>
        /// Builds the default logger factory adapter.
        /// </summary>
        /// <returns></returns>
		private static ILoggerFactoryAdapter BuildDefaultLoggerFactoryAdapter()
		{
            return new NoOpLoggerFactoryAdapter();
		}
	}
}

