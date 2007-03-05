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
		/// 
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return _logger.IsInfoEnabled; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return _logger.IsWarnEnabled; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsErrorEnabled
		{
            get { return _logger.IsErrorEnabled; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFatalEnabled
		{
            get { return _logger.IsFatalEnabled; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDebugEnabled
		{
            get { return _logger.IsDebugEnabled; }
		}

		/// <summary>
		/// 
		/// </summary>
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
			_logger.LogException(LogLevelNLog.Info, message.ToString(), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
            _logger.Log(LogLevelNLog.Info, message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
            _logger.LogException(LogLevelNLog.Debug, message.ToString(), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
            _logger.Log(LogLevelNLog.Debug, message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
            _logger.LogException(LogLevelNLog.Warn, message.ToString(), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
            _logger.Log(LogLevelNLog.Warn, message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Trace(object message, Exception e)
		{
            _logger.LogException(LogLevelNLog.Trace, message.ToString(), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Trace(object message)
		{
            _logger.Log(LogLevelNLog.Trace, message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Fatal(object message, Exception e)
		{
            _logger.LogException(LogLevelNLog.Fatal, message.ToString(), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(object message)
		{
            _logger.Log(LogLevelNLog.Fatal, message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
            _logger.LogException(LogLevelNLog.Error, message.ToString(), e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
            _logger.Log(LogLevelNLog.Error, message);
		}

		#endregion
	}
}