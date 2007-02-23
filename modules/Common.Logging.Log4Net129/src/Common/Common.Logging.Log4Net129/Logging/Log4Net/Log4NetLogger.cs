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
using log4net.Core;

namespace Common.Logging.Log4Net
{
	/// <remarks>
	/// Log4net is capable of outputting extended debug information about where the current 
	/// message was generated: class name, method name, file, line, etc. Log4net assumes that the location
	/// information should be gathered relative to where Debug() was called. 
	/// When using Common.Logging, Debug() is called in Common.Logging.Log4Net.Log4NetLogger. This means that
	/// the location information will indicate that Common.Logging.Log4Net.Log4NetLogger always made
	/// the call to Debug(). We need to know where Common.Logging.ILog.Debug()
	/// was called. To do this we need to use the log4net.ILog.Logger.Log method and pass in a Type telling
	/// log4net where in the stack to begin looking for location information.
	/// </remarks>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: Log4NetLogger.cs,v 1.1 2006/11/13 07:36:27 markpollack Exp $</version>
	public class Log4NetLogger : ILog
	{
		#region Fields

		private ILogger _logger = null;
		private readonly static Type declaringType = typeof(Log4NetLogger);

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="log"></param>
		internal Log4NetLogger(log4net.ILog log )
		{
			_logger = log.Logger;
		}

		#region ILog Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return _logger.IsEnabledFor(Level.Info); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return _logger.IsEnabledFor(Level.Warn); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return _logger.IsEnabledFor(Level.Error); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return _logger.IsEnabledFor(Level.Fatal); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return _logger.IsEnabledFor(Level.Debug); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsTraceEnabled
		{
			get { return _logger.IsEnabledFor(Level.Trace); }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Info(object message, Exception e)
		{
			_logger.Log(declaringType, Level.Info, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
			_logger.Log(declaringType, Level.Info, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
			_logger.Log(declaringType, Level.Debug, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
			_logger.Log(declaringType, Level.Debug, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
			_logger.Log(declaringType, Level.Warn, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
			_logger.Log(declaringType, Level.Warn, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Trace(object message, Exception e)
		{
			_logger.Log(declaringType, Level.Trace, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Trace(object message)
		{
			_logger.Log(declaringType, Level.Trace, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Fatal(object message, Exception e)
		{
			_logger.Log(declaringType, Level.Fatal, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(object message)
		{
			_logger.Log(declaringType, Level.Fatal, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
			_logger.Log(declaringType, Level.Error, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
			_logger.Log(declaringType, Level.Error, message, null);
		}

		#endregion
	}
}