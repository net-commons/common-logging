#region License

/*
 * Copyright © 2002-2009 the original author or authors.
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
using FormatMessageCallback = System.Action<Common.Logging.FormatMessageHandler>;

namespace Common.Logging.Factory
{
    /// <summary>
    /// Provides base implementation suitable for almost all logger adapters
    /// </summary>
    /// <author>Erich Eichinger</author>
#if !PORTABLE
    [Serializable]
#endif
    public abstract class AbstractLogger : ILog
    {
        #region FormatMessageCallbackFormattedMessage

        /// <summary>
        /// Format message on demand.
        /// </summary>
        protected class FormatMessageCallbackFormattedMessage
        {
            private volatile string cachedMessage;

            private readonly IFormatProvider formatProvider;
            private readonly FormatMessageCallback formatMessageCallback;

            /// <summary>
            /// Initializes a new instance of the <see cref="FormatMessageCallbackFormattedMessage"/> class.
            /// </summary>
            /// <param key="formatMessageCallback">The format message callback.</param>
            public FormatMessageCallbackFormattedMessage(FormatMessageCallback formatMessageCallback)
            {
                this.formatMessageCallback = formatMessageCallback;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FormatMessageCallbackFormattedMessage"/> class.
            /// </summary>
            /// <param key="formatProvider">The format provider.</param>
            /// <param key="formatMessageCallback">The format message callback.</param>
            public FormatMessageCallbackFormattedMessage(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
            {
                this.formatProvider = formatProvider;
                this.formatMessageCallback = formatMessageCallback;
            }

            /// <summary>
            /// Calls <see cref="formatMessageCallback"/> and returns result.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (cachedMessage == null && formatMessageCallback != null)
                {
                    formatMessageCallback(FormatMessage);
                }
                return cachedMessage;
            }

            private string FormatMessage(string format, params object[] args)
            {
                cachedMessage = string.Format(formatProvider, format, args);
                return cachedMessage;
            }
        }

        #endregion

        #region StringFormatFormattedMessage

        /// <summary>
        /// Format string on demand.
        /// </summary>
        protected class StringFormatFormattedMessage
        {
            private volatile string cachedMessage;

            private readonly IFormatProvider FormatProvider;
            private readonly string Message;
            private readonly object[] Args;

            /// <summary>
            /// Initializes a new instance of the <see cref="StringFormatFormattedMessage"/> class.
            /// </summary>
            /// <param key="formatProvider">The format provider.</param>
            /// <param key="message">The message.</param>
            /// <param key="args">The args.</param>
            public StringFormatFormattedMessage(IFormatProvider formatProvider, string message, params object[] args)
            {
                FormatProvider = formatProvider;
                Message = message;
                Args = args;
            }

            /// <summary>
            /// Runs <see cref="string.Format(System.IFormatProvider,string,object[])"/> on supplied arguemnts.
            /// </summary>
            /// <returns>string</returns>
            public override string ToString()
            {
                if (cachedMessage == null && Message != null)
                {
                    cachedMessage = string.Format(FormatProvider, Message, Args);
                }
                return cachedMessage;
            }
        }

        #endregion

        /// <summary>
        /// Represents a method responsible for writing a message to the log system.
        /// </summary>
        protected delegate void WriteHandler(LogLevel level, object message, Exception exception);

        /// <summary>
        /// Holds the method for writing a message to the log system.
        /// </summary>
        private readonly WriteHandler Write;

        /// <summary>
        /// Creates a new logger instance using <see cref="WriteInternal"/> for 
        /// writing log events to the underlying log system.
        /// </summary>
        /// <seealso cref="GetWriteHandler"/>
        protected AbstractLogger()
        {
            Write = GetWriteHandler();
            if (Write == null)
            {
                Write = new WriteHandler(WriteInternal);
            }
        }

        /// <summary>
        /// Override this method to use a different method than <see cref="WriteInternal"/> 
        /// for writing log events to the underlying log system.
        /// </summary>
        /// <remarks>
        /// Usually you don't need to override thise method. The default implementation returns
        /// <c>null</c> to indicate that the default handler <see cref="WriteInternal"/> should be 
        /// used.
        /// </remarks>
        protected virtual WriteHandler GetWriteHandler()
        {
            return null;
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <remarks>
        /// Override this in your derived class to comply with the underlying logging system
        /// </remarks>
        public abstract bool IsTraceEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <remarks>
        /// Override this in your derived class to comply with the underlying logging system
        /// </remarks>
        public abstract bool IsDebugEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <remarks>
        /// Override this in your derived class to comply with the underlying logging system
        /// </remarks>
        public abstract bool IsInfoEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <remarks>
        /// Override this in your derived class to comply with the underlying logging system
        /// </remarks>
        public abstract bool IsWarnEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <remarks>
        /// Override this in your derived class to comply with the underlying logging system
        /// </remarks>
        public abstract bool IsErrorEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <remarks>
        /// Override this in your derived class to comply with the underlying logging system
        /// </remarks>
        public abstract bool IsFatalEnabled { get; }

        /// <summary>
        /// Actually sends the message to the underlying log system.
        /// </summary>
        /// <param key="level">the level of this log event.</param>
        /// <param key="message">the message to log</param>
        /// <param key="exception">the exception to log (may be null)</param>
        protected abstract void WriteInternal(LogLevel level, object message, Exception exception);

        #region Trace

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        public virtual void Trace(object message)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        public virtual void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        public virtual void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        public virtual void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        public virtual void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        public virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Trace(FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        public virtual void Trace(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        public virtual void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Debug

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        public virtual void Debug(object message)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack Debug of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack Debug.</param>
        public virtual void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        public virtual void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        public virtual void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        public virtual void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        public virtual void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Debug(FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Debug.</param>
        public virtual void Debug(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Debug.</param>
        public virtual void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Info

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        public virtual void Info(object message)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack Info of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack Info.</param>
        public virtual void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        public virtual void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        public virtual void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        public virtual void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        public virtual void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Info(FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Info.</param>
        public virtual void Info(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Info.</param>
        public virtual void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Warn

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        public virtual void Warn(object message)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack Warn of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack Warn.</param>
        public virtual void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Warnrmation.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        public virtual void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Warnrmation.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        public virtual void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        public virtual void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        public virtual void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Warn(FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Warn.</param>
        public virtual void Warn(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Warn.</param>
        public virtual void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Error

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        public virtual void Error(object message)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack Error of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack Error.</param>
        public virtual void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Errorrmation.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        public virtual void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Errorrmation.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        public virtual void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        public virtual void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        public virtual void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Error(FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Error.</param>
        public virtual void Error(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Error.</param>
        public virtual void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        public virtual void Fatal(object message)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, message, null);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack Fatal of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack Fatal.</param>
        public virtual void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Fatalrmation.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        public virtual void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting Fatalrmation.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        public virtual void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        public virtual void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        public virtual void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Fatal(FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Fatal.</param>
        public virtual void Fatal(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public virtual void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack Fatal.</param>
        public virtual void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        #endregion

        /// <summary>
        /// Returns the global context for variables
        /// </summary>
        public virtual IVariablesContext GlobalVariablesContext
        {
            get { return new Simple.NoOpVariablesContext(); }
        }

        /// <summary>
        /// Returns the thread-specific context for variables
        /// </summary>
        public virtual IVariablesContext ThreadVariablesContext
        {
            get { return new Simple.NoOpVariablesContext(); }
        }
    }
}