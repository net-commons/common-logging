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
using System.Globalization;
using System.Text;

namespace Common.Logging.Simple
{
	/// <summary>
	/// Sends log messages to <see cref="Console.Out" />.
	/// </summary>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: ConsoleOutLogger.cs,v 1.1 2006/11/13 07:17:55 markpollack Exp $</version>
	public class ConsoleOutLogger : ILog
	{
		private bool _showDateTime = false;
		private bool _showLogName = false;
		private string _logName = string.Empty;
		private LogLevel _currentLogLevel = LogLevel.All;
		private string _dateTimeFormat = string.Empty;
		private bool _hasDateTimeFormat = false;

		/// <summary>
		/// Creates and initializes a logger that writes messages to <see cref="Console.Out" />.
		/// </summary>
		/// <param name="logName">The name, usually type name of the calling class, of the logger.</param>
		/// <param name="logLevel">The current logging threshold. Messages recieved that are beneath this threshold will not be logged.</param>
		/// <param name="showDateTime">Include the current time in the log message.</param>
		/// <param name="showLogName">Include the instance name in the log message.</param>
		/// <param name="dateTimeFormat">The date and time format to use in the log message.</param>
		public ConsoleOutLogger( string logName, LogLevel logLevel
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
		/// Do the actual logging by constructing the log message using a <see cref="StringBuilder" /> then
		/// sending the output to <see cref="Console.Out" />.
		/// </summary>
		/// <param name="level">The <see cref="LogLevel" /> of the message.</param>
		/// <param name="message">The log message.</param>
		/// <param name="e">An optional <see cref="Exception" /> associated with the message.</param>
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
			sb.Append( ("[" + level.ToString().ToUpper() + "]").PadRight( 8 ) );

			// Append the name of the log instance if so configured
			if ( _showLogName )
			{
				sb.Append( _logName ).Append( " - " );
			}

			// Append the message
			sb.Append( message );

			// Append stack trace if not null
			if ( e != null )
			{
				sb.Append(Environment.NewLine).Append( e.ToString() );
			}

			// Print to the appropriate destination
			Console.Out.WriteLine( sb.ToString() );
		}

		/// <summary>
		/// Determines if the given log level is currently enabled.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		private bool IsLevelEnabled( LogLevel level )
		{
			int iLevel = (int)level;
			int iCurrentLogLevel = (int)_currentLogLevel;
		
			// return iLevel.CompareTo(iCurrentLogLevel); better ???
			return ( iLevel >= iCurrentLogLevel );
		}

		#region ILog Members

		/// <summary>
		/// Log a <see cref="LogLevel.Debug" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Debug(object message)
		{
			Debug( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Debug" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Debug(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Debug ) )
			{
				Write( LogLevel.Debug, message, e );	
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Error" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Error(object message)
		{
			Error( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Error" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Error(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Error ) )
			{
				Write( LogLevel.Error, message, e );	
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Fatal" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Fatal(object message)
		{
			Fatal( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Fatal" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Fatal(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Fatal ) )
			{
				Write( LogLevel.Fatal, message, e );
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Info" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Info(object message)
		{
			Info( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Info" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Info(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Info ) )
			{
				Write( LogLevel.Info, message, e );
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Warn" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Warn(object message)
		{
			Warn( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Warn" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Warn(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Warn ) )
			{
				Write( LogLevel.Warn, message, e );
			}
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Debug" />. If it is, all messages will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return IsLevelEnabled( LogLevel.Debug ); }
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Error" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Error" /> and <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return IsLevelEnabled( LogLevel.Error ); }
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Fatal" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return IsLevelEnabled( LogLevel.Fatal ); }
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Info" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Info" />, <see cref="LogLevel.Warn" />, <see cref="LogLevel.Error" />, and 
		/// <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return IsLevelEnabled( LogLevel.Info ); }
		}


		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Warn" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Warn" />, <see cref="LogLevel.Error" />, and <see cref="LogLevel.Fatal" /> 
		/// will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return IsLevelEnabled( LogLevel.Warn ); }
		}

		#endregion
	}
}
