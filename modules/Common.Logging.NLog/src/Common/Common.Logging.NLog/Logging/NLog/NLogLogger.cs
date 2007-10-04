#region License

/*
 * Copyright © 2002-2007 the original author or authors.
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

#region Imports

using System;
using NLog;
using LogLevelNLog = NLog.LogLevel;
using LoggerNLog = NLog.Logger;


#endregion

namespace Common.Logging.NLog
{
    /// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to NLog.
    /// </summary>
	/// <remarks>
    /// NLog is a .NET logging library designed with simplicity and flexibility in mind.
    /// http://www.nlog-project.org/
	/// </remarks>
    /// <author>Bruno Baia</author>
    /// <version>$Id: $</version>
	public class NLogLogger : ILog
	{
		#region Fields

        private LoggerNLog _logger = null;
        private readonly static Type declaringType = typeof(NLogLogger);

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="logger"></param>
        internal NLogLogger(LoggerNLog logger)
		{
			_logger = logger;
		}

		#region ILog Members


        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is info enabled; otherwise, <c>false</c>.
        /// </value>
		public bool IsInfoEnabled
		{
			get { return _logger.IsInfoEnabled; }
		}


        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is warn enabled; otherwise, <c>false</c>.
        /// </value>
		public bool IsWarnEnabled
		{
			get { return _logger.IsWarnEnabled; }
		}

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is error enabled; otherwise, <c>false</c>.
        /// </value>
		public bool IsErrorEnabled
		{
            get { return _logger.IsErrorEnabled; }
		}


        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fatal enabled; otherwise, <c>false</c>.
        /// </value>
		public bool IsFatalEnabled
		{
            get { return _logger.IsFatalEnabled; }
		}

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
		public bool IsDebugEnabled
		{
            get { return _logger.IsDebugEnabled; }
		}


        /// <summary>
        /// Gets a value indicating whether this instance is trace enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is trace enabled; otherwise, <c>false</c>.
        /// </value>
		public bool IsTraceEnabled
		{
            get { return _logger.IsTraceEnabled; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Info(object message, Exception e)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Info, _logger.Name, null, (message == null ? null : message.ToString()), null, e);
		    _logger.Log(declaringType, logEvent);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Info, _logger.Name, null, (message == null ? null : message.ToString()), null, null);
            _logger.Log(declaringType, logEvent);		   
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
            //LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Debug, _logger.Name, null, message.ToString(), null, e);
            //_logger.Log(declaringType, logEvent);
            _logger.DebugException((message == null ? null : message.ToString()), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Debug, _logger.Name, null, (message == null ? null : message.ToString()), null, null);
            _logger.Log(declaringType, logEvent);	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Warn, _logger.Name, null, (message == null ? null : message.ToString()), null, e);
            _logger.Log(declaringType, logEvent);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Warn, _logger.Name, null, (message == null ? null : message.ToString()), null, null);
            _logger.Log(declaringType, logEvent);	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Trace(object message, Exception e)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Trace, _logger.Name, null, (message == null ? null : message.ToString()), null, e);
            _logger.Log(declaringType, logEvent);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Trace(object message)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Trace, _logger.Name, null, (message == null ? null : message.ToString()), null, null);
            _logger.Log(declaringType, logEvent);
		}


        /// <summary>
        /// Logs the specified message and exception at the Fatal log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
		public void Fatal(object message, Exception e)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Fatal, _logger.Name, null, (message == null ? null : message.ToString()), null, e);
            _logger.Log(declaringType, logEvent);
		}


        /// <summary>
        /// Logs the specified message at the Fatal log level.
        /// </summary>
        /// <param name="message">The message.</param>
		public void Fatal(object message)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Fatal, _logger.Name, null, (message == null ? null : message.ToString()), null, null);
            _logger.Log(declaringType, logEvent);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Error, _logger.Name, null, (message == null ? null : message.ToString()), null, e);
            _logger.Log(declaringType, logEvent);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
            LogEventInfo logEvent = new LogEventInfo(LogLevelNLog.Error, _logger.Name, null, (message == null ? null : message.ToString()), null, null);
            _logger.Log(declaringType, logEvent);
		}

		#endregion
	}
}