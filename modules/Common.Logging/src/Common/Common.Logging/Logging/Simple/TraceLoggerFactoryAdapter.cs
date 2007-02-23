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

namespace Common.Logging.Simple
{
	/// <summary>
    /// Factory for creating <see cref="ILog" /> instances that send 
    /// everything to the system.Diagnostics.Trace output stream.
	/// </summary>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: TraceLoggerFactoryAdapter.cs,v 1.2 2006/12/04 22:11:51 oakinger Exp $</version>
	public class TraceLoggerFactoryAdapter: ILoggerFactoryAdapter 
	{
		private Hashtable _logs = new Hashtable();
		private LogLevel _Level = LogLevel.All;
		private bool _showDateTime = true;
		private bool _showLogName = true;
		private string _dateTimeFormat = string.Empty;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="properties"></param>
		public TraceLoggerFactoryAdapter(NameValueCollection properties)
		{
			try
			{
				_Level = (LogLevel)Enum.Parse( typeof(LogLevel), properties["level"], true );
			}
			catch ( Exception )
			{
				_Level = LogLevel.All;
			}
			try
			{
				_showDateTime = bool.Parse( properties["showDateTime"] );
			}
			catch ( Exception )
			{
				_showDateTime = true;
			}
			try 
			{
				_showLogName = bool.Parse( properties["showLogName"] );
			}
			catch ( Exception )
			{
				_showLogName = true;
			}
			_dateTimeFormat =  properties["dateTimeFormat"];
		}

		#region ILoggerFactoryAdapter Members

		/// <summary>
		/// Get a ILog instance by type 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ILog GetLogger(Type type)
		{
			return GetLogger( type.FullName );
		}

		/// <summary>
		/// Get a ILog instance by type name 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ILog GetLogger(string name)
		{
			lock(_logs)
			{
				ILog log = _logs[name] as ILog;
				if ( log == null )
				{
					log = new TraceLogger( name, _Level, _showDateTime, _showLogName, _dateTimeFormat );
					_logs.Add( name, log );
				}
				return log;
			}
		}

		#endregion
	}
}
