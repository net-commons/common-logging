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
using Common.Logging.Factory;
using FormatMessageCallback = System.Action<Common.Logging.FormatMessageHandler>;
using LogLevelNLog = NLog.LogLevel;
using LoggerNLog = NLog.Logger;

namespace Common.Logging.NLog
{
    /// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to Serilog 1.5.14
    /// </summary>
    /// <remarks>
    /// Unlike other logging libraries, Serilog is built with powerful structured event data in mind.
    /// http://serilog.net/
    /// </remarks>
    /// <author>Aaron Mell</author>
    public partial class NLogLogger : AbstractLogger
    {

        #region SerilogSerilogFormatMessageCallbackFormattedMessage

        /// <summary>
        /// Format message on demand.
        /// </summary>
        protected class NLogFormatMessageCallbackFormattedMessage : FormatMessageCallbackFormattedMessage
        {
            /// <summary>
            /// Calls FormatMessageCallbackFormattedMessage.formatMessageCallback and returns result.
            /// This allows Serilog to work propery, since it has its own formatting.
            /// </summary>
            /// <returns></returns>
            public string ToParameters(out object[] arguments)
            {
                if (cachedFormat == null && formatMessageCallback != null)
                {
                    //Calling this instead of a new function, because the return value must be a string.
                    formatMessageCallback(FormatMessage);
                }

                arguments = cachedArguments;
                return cachedFormat;
            }


            /// <summary>
            /// Initializes a new instance of the <see cref="NLogFormatMessageCallbackFormattedMessage"/> class.
            /// </summary>
            /// <param name="formatMessageCallback">The format message callback.</param>
            public NLogFormatMessageCallbackFormattedMessage(FormatMessageCallback formatMessageCallback) : base(formatMessageCallback)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="NLogFormatMessageCallbackFormattedMessage"/> class.
            /// </summary>
            /// <param name="formatProvider">The format provider.</param>
            /// <param name="formatMessageCallback">The format message callback.</param>
            public NLogFormatMessageCallbackFormattedMessage(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) : base(formatProvider, formatMessageCallback)
            {
            }
        }

        #endregion

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
                _logger.Trace("{0}", message);
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
                _logger.Trace(exception, "{0}", message);
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
                _logger.Trace(formatProvider, format, args);
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
                _logger.Trace(exception, formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
                _logger.Trace(format, args);
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
                _logger.Trace(exception, format, args);
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Trace(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Trace(exception, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Trace(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Trace(exception, format, arguments);
            }
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
                _logger.Debug("{0}", message);
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
                _logger.Debug(exception, "{0}", message);
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
                _logger.Debug(formatProvider, format, args);
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
                _logger.Debug(exception, formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                _logger.Debug(format, args);
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
                _logger.Debug(exception, format, args);
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Debug(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Debug(exception, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Debug(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Debug(exception, format, arguments);
            }
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
                _logger.Info("{0}", message);
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
                _logger.Info(exception, "{0}", message);
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
                _logger.Info(formatProvider, format, args);
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
                _logger.Info(exception, formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                _logger.Info(format, args);
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
                _logger.Info(exception, format, args);
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Info(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Info(exception, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Info(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Info(exception, format, arguments);
            }
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
                _logger.Warn("{0}", message);
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
                _logger.Warn(exception, "{0}", message);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public override void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                _logger.Warn(formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public override void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                _logger.Warn(exception, formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                _logger.Warn(format, args);
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
                _logger.Warn(exception, format, args);
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Warn(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Warn(exception, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Warn(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Warn(exception, format, arguments);
            }
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
                _logger.Error(message.ToString());
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
                _logger.Error(exception, message.ToString());
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
                _logger.Error(format, args);
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
                _logger.Error(exception, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                _logger.Error(format, args);
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
                _logger.Error(exception, format, args);
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Error(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Error(exception, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Error(formatProvider, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Error(exception, formatProvider, format, arguments);
            }
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
                _logger.Fatal("{0}", message);
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
                _logger.Fatal(exception, "{0}", message);
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
                _logger.Fatal(formatProvider, format, args);
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
                _logger.Fatal(exception, formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public override void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
                _logger.Fatal(format, args);
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
                _logger.Fatal(exception, format, args);
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Fatal(format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Fatal(exception, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Fatal(formatProvider, format, arguments);
            }
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
            {
                object[] arguments;
                var format = new NLogFormatMessageCallbackFormattedMessage(formatMessageCallback).ToParameters(out arguments);
                _logger.Fatal(exception, formatProvider, format, arguments);
            }
        }

        #endregion

        #endregion


        /// <summary>
        /// Actually sends the message to the underlying log system.
        /// </summary>
        /// <param name="level">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            //Do nothing here. This method is not compatible with Serilog            
        }
    }
}
