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

namespace Common.Logging
{
	/// <summary>
	/// The 7 logging levels used by Log are (in order): 
	/// </summary>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: ILog.cs,v 1.1 2006/11/13 07:17:55 markpollack Exp $</version>
	public enum LogLevel
	{
		/// <summary>
		/// 
		/// </summary>
		All   = 0,
		/// <summary>
		/// 
		/// </summary>
		Debug = 1,
		/// <summary>
		/// 
		/// </summary>
		Info  = 2,
		/// <summary>
		/// 
		/// </summary>
		Warn  = 3,
		/// <summary>
		/// 
		/// </summary>
		Error = 4,
		/// <summary>
		///
		/// </summary>
		Fatal = 5,
		/// <summary>
		/// Do not log anything.
		/// </summary>
		Off  = 6,
	}

	/// <summary>
	/// A simple logging interface abstracting logging APIs. 
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Debug"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		void Debug( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Debug"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		void Debug( object message, Exception exception );

//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Debug"/> level.
//		/// </summary>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void DebugFormat(string format, params object[] args); 
//
//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Debug"/> level.
//		/// </summary>
//		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void DebugFormat(IFormatProvider provider, string format, params object[] args);

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Error"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		void Error( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Error"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		void Error( object message, Exception exception );

//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Error"/> level.
//		/// </summary>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void ErrorFormat(string format, params object[] args); 
//
//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Error"/> level.
//		/// </summary>
//		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void ErrorFormat(IFormatProvider provider, string format, params object[] args);

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Fatal"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		void Fatal( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Fatal"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		void Fatal( object message, Exception exception );

//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Fatal"/> level.
//		/// </summary>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void FatalFormat(string format, params object[] args); 
//
//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Fatal"/> level.
//		/// </summary>
//		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void FatalFormat(IFormatProvider provider, string format, params object[] args);

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Info"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		void Info( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Info"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		void Info( object message, Exception exception );

//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Info"/> level.
//		/// </summary>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void InfoFormat(string format, params object[] args); 

//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Info"/> level.
//		/// </summary>
//		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void InfoFormat(IFormatProvider provider, string format, params object[] args);

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Warn"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		void Warn( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Warn"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		void Warn( object message, Exception exception );

//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Warn"/> level.
//		/// </summary>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void WarnFormat(string format, params object[] args); 
//
//		/// <summary>
//		/// Logs a formatted message string with the <see cref="LogLevel.Warn"/> level.
//		/// </summary>
//		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
//		/// <param name="format">A String containing zero or more format items</param>
//		/// <param name="args">An Object array containing zero or more objects to format</param>
//		void WarnFormat(IFormatProvider provider, string format, params object[] args);

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="LogLevel.Debug"/> level.
		/// </summary>
		bool IsDebugEnabled
		{
			get;
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="LogLevel.Error"/> level.
		/// </summary>
		bool IsErrorEnabled
		{
			get;
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="LogLevel.Fatal"/> level.
		/// </summary>
		bool IsFatalEnabled
		{
			get;
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="LogLevel.Info"/> level.
		/// </summary>
		bool IsInfoEnabled
		{
			get;
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="LogLevel.Warn"/> level.
		/// </summary>
		bool IsWarnEnabled
		{
			get;
		}
	}
}
