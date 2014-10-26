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

using System;
using System.Diagnostics;
using Common.Logging.Factory;
using NLog;
using LogLevelNLog = NLog.LogLevel;
using LoggerNLog = NLog.Logger;
using FormatMessageCallback = System.Action<Common.Logging.FormatMessageHandler>;

namespace Common.Logging.NLog
{
    /// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to NLog 1.0.0.505-2.0.
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
        // Stack unwinding algorithm was changed in NLog2 (now it checks for system assemblies and logger type)
        // so we need this workaround to make it display correct stack trace.
        private readonly static Type declaringType = typeof(NLogLogger);

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

        #region Trace

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public override void Trace(object message)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public override void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public new virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Trace(FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public override void Trace(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public override void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Debug

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public override void Debug(object message)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack Debug of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public override void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public override void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Debug(FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public override void Debug(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public override void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Info

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public override void Info(object message)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack Info of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public override void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public override void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Info(FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public override void Info(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public override void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Warn

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public override void Warn(object message)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack Warn of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public override void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Warnrmation.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Warnrmation.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public override void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Warn(FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public override void Warn(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public override void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Error

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public override void Error(object message)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack Error of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public override void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Errorrmation.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Errorrmation.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public override void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Error(FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public override void Error(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public override void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public override void Fatal(object message)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack Fatal of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public override void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Fatalrmation.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Fatalrmation.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public override void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Fatal(FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public override void Fatal(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public override void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public override void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

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

        /// <summary>
        /// Returns the global context for variables
        /// </summary>
        public override IVariablesContext GlobalVariablesContext
        {
            get { return new NLogGlobalVariablesContext(); }
        }

        /// <summary>
        /// Returns the thread-specific context for variables
        /// </summary>
        public override IVariablesContext ThreadVariablesContext
        {
            get { return new NLogThreadVariablesContext(); }
        }
    }
}