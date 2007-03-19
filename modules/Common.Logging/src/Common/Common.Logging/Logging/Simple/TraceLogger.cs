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
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Common.Logging.Simple
{
	/// <summary>
	/// Logger sending everything to the trace output stream.
	/// </summary>
	/// <author>Gilles Bayon</author>
    /// <version>$Id: TraceLogger.cs,v 1.1 2006/11/13 07:17:55 markpollack Exp $</version>
    [Serializable]
    public class TraceLogger: ILog
	{
		private bool _showDateTime = false;
		private bool _showLogName = false;
		private string _logName = string.Empty;
		private LogLevel _currentLogLevel = LogLevel.All;
		private string _dateTimeFormat = string.Empty;
		private bool _hasDateTimeFormat = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logName"></param>
		/// <param name="logLevel"></param>
		/// <param name="showDateTime">Include the current time in the log message </param>
		/// <param name="showLogName">Include the instance name in the log message</param>
		/// <param name="dateTimeFormat">The date and time format to use in the log message </param>
		public TraceLogger( string logName, LogLevel logLevel
			, bool showDateTime, bool showLogName, string dateTimeFormat)
		{
			_logName = logName;
			_currentLogLevel = logLevel;
			_showDateTime = showDateTime;
			_showLogName = showLogName;
			_dateTimeFormat = dateTimeFormat;

			if (_dateTimeFormat != null && _dateTimeFormat.Length > 0)
			{
				_hasDateTimeFormat = true;
			}
		}

		/// <summary>
		/// Do the actual logging.
		/// This method assembles the message and write
		/// the content of the message accumulated in the specified
		/// StringBuffer to the appropriate output destination. The
		/// default implementation writes to System.Console.Error.<p/>
		/// </summary>
		/// <param name="level"></param>
		/// <param name="message"></param>
		/// <param name="e"></param>
		private void Write( LogLevel level, object message, Exception e )
		{
			// Use a StringBuilder for better performance
			StringBuilder sb = new StringBuilder();
			// Append date-time if so configured
			if ( _showDateTime )
			{
				if ( _hasDateTimeFormat )
				{
					sb.Append( DateTime.Now.ToString( _dateTimeFormat, CultureInfo.InvariantCulture ));
				}
				else
				{
					sb.Append( DateTime.Now );
				}
				
				sb.Append( " " );
			}	
			// Append a readable representation of the log level
            sb.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));

			// Append the name of the log instance if so configured
			if ( _showLogName )
			{
				sb.Append( _logName ).Append( " - " );
			}

			// Append the message
            sb.Append(message);

			// Append stack trace if not null
			if ( e != null )
			{
				sb.AppendFormat( "\n{0}", e.ToString() );
			}

			// Print to the appropriate destination
			Trace.WriteLine( sb.ToString() );			
		}

		/// <summary>
		/// Is the given log level currently enabled ?
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		private bool IsLevelEnabled( LogLevel level )
		{
			int iLevel = (int)level;
			int iCurrentLogLevel = (int)_currentLogLevel;

			return ( iLevel >= iCurrentLogLevel );
		}

		#region ILog Members

		/// <summary>
		/// Log a debug message.
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
			Debug( message, null );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Debug ) )
			{
				Write( LogLevel.Debug, message, e );	
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
			Error( message, null );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Error ) )
			{
				Write( LogLevel.Error, message, e );	
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(object message)
		{
			Fatal( message, null );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Fatal(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Fatal ) )
			{
				Write( LogLevel.Fatal, message, e );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
			Info( message, null );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Info(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Info ) )
			{
				Write( LogLevel.Info, message, e );
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
			Warn( message, null );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Warn ) )
			{
				Write( LogLevel.Warn, message, e );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return IsLevelEnabled( LogLevel.Debug ); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return IsLevelEnabled( LogLevel.Error ); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return IsLevelEnabled( LogLevel.Fatal ); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return IsLevelEnabled( LogLevel.Info ); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return IsLevelEnabled( LogLevel.Warn ); }
		}

		#endregion
	}
}

