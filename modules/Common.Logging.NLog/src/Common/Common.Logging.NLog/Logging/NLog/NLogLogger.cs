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
using System.Reflection;
using Common.Logging.Factory;
using NLog;
using LogLevelNLog = NLog.LogLevel;
using LoggerNLog = NLog.Logger;


#endregion

namespace Common.Logging.NLog
{
    /// <summary>
	/// Concrete implementation of <see cref="ILog"/> interface specific to NLog 1.0.0.505.
    /// </summary>
	/// <remarks>
    /// NLog is a .NET logging library designed with simplicity and flexibility in mind.
    /// http://www.nlog-project.org/
	/// </remarks>
    /// <author>Bruno Baia</author>
	public class NLogLogger : AbstractLogger
	{
		#region Fields

        private readonly LoggerNLog _logger;
        private readonly static Type declaringType = typeof(AbstractLogger);

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
        protected internal NLogLogger(LoggerNLog logger)
		{
			_logger = logger;
		}

		#region ILog Members

        /// <summary>
        /// Gets a value indicating whether this instance is trace enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is trace enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool IsTraceEnabled
        {
            get { return _logger.IsTraceEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is info enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool IsInfoEnabled
		{
			get { return _logger.IsInfoEnabled; }
		}


        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is warn enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool IsWarnEnabled
		{
			get { return _logger.IsWarnEnabled; }
		}

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is error enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool IsErrorEnabled
		{
            get { return _logger.IsErrorEnabled; }
		}

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fatal enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool IsFatalEnabled
		{
            get { return _logger.IsFatalEnabled; }
		}

		#endregion

        /// <summary>
        /// Actually sends the message to the underlying log system.
        /// </summary>
        /// <param name="logLevel">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        protected override void WriteInternal(LogLevel logLevel, object message, Exception exception)
        {
            LogLevelNLog level = GetLevel(logLevel);
            LogEventInfo logEvent = new LogEventInfo(level, _logger.Name, null, "{0}", new object[] { message }, exception);
            _logger.Log(declaringType, logEvent);
        }

        private static LogLevelNLog GetLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return LogLevelNLog.Trace;
                case LogLevel.Trace:
                    return LogLevelNLog.Trace;
                case LogLevel.Debug:
                    return LogLevelNLog.Debug;
                case LogLevel.Info:
                    return LogLevelNLog.Info;
                case LogLevel.Warn:
                    return LogLevelNLog.Warn;
                case LogLevel.Error:
                    return LogLevelNLog.Error;
                case LogLevel.Fatal:
                    return LogLevelNLog.Fatal;
                case LogLevel.Off:
                    return LogLevelNLog.Off;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, "unknown log level");
            }
        }
    }
}