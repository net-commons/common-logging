using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Logging.MultipleLogger
{
    /// <summary>
    /// Multi logger logs an event to multipe loggers
    /// </summary>
    /// <seealso cref="Common.Logging.ILog" />
    public class MultiLogger : ILog
    {
        private readonly List<ILog> _loggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLogger"/> class.
        /// </summary>
        /// <param name="loggers">The loggers.</param>
        public MultiLogger(List<ILog> loggers)
        {
            _loggers = loggers;
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Debug.</param>
        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Debug(formatProvider, formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Debug(formatProvider, formatMessageCallback));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Debug(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Debug(formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Debug(Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Debug(formatMessageCallback));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Debug(object message, Exception exception)
        {
            _loggers.ForEach(l => l.Debug(message, exception));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Debug(object message)
        {
            _loggers.ForEach(l => l.Debug(message));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(formatProvider, format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args"></param>
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(formatProvider, format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args">the list of format arguments</param>
        public void DebugFormat(string format, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Error.</param>
        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Error(formatProvider, formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Error(formatProvider, formatMessageCallback));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Error(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Error(formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Error(Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Error(formatMessageCallback));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Error(object message, Exception exception)
        {
            _loggers.ForEach(l => l.Error(message, exception));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Error(object message)
        {
            _loggers.ForEach(l => l.Error(message));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(formatProvider, format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args"></param>
        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(formatProvider, format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args">the list of format arguments</param>
        public void ErrorFormat(string format, params object[] args)
        {
            _loggers.ForEach(l => l.DebugFormat(format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Fatal.</param>
        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Fatal(formatProvider, formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Fatal(formatProvider, formatMessageCallback));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Fatal(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Fatal(formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Fatal(Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Fatal(formatMessageCallback));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Fatal(object message, Exception exception)
        {
            _loggers.ForEach(l => l.Fatal(message, exception));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Fatal(object message)
        {
            _loggers.ForEach(l => l.Fatal(message));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.FatalFormat(formatProvider, format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args"></param>
        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            _loggers.ForEach(l => l.FatalFormat(formatProvider, format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.FatalFormat(format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args">the list of format arguments</param>
        public void FatalFormat(string format, params object[] args)
        {
            _loggers.ForEach(l => l.FatalFormat(format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Info.</param>
        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Info(formatProvider, formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Info(formatProvider, formatMessageCallback));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Info(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Info(formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Info(Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Info(formatMessageCallback));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Info(object message, Exception exception)
        {
            _loggers.ForEach(l => l.Info(message, exception));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Info(object message)
        {
            _loggers.ForEach(l => l.Info(message));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.InfoFormat(formatProvider, format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args"></param>
        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            _loggers.ForEach(l => l.InfoFormat(formatProvider, format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.InfoFormat(format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args">the list of format arguments</param>
        public void InfoFormat(string format, params object[] args)
        {
            _loggers.ForEach(l => l.InfoFormat(format, args));
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return _loggers.Any(x => x.IsDebugEnabled); }
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Error"/> level.
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return _loggers.Any(x => x.IsErrorEnabled); }
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return _loggers.Any(x => x.IsFatalEnabled); }
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Info"/> level.
        /// </summary>
        public bool IsInfoEnabled
        {
            get { return _loggers.Any(x => x.IsInfoEnabled); }
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        public bool IsTraceEnabled
        {
            get { return _loggers.Any(x => x.IsTraceEnabled); }
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return _loggers.Any(x => x.IsWarnEnabled); }
        }

        /// <summary>
        /// Returns the global context for variables
        /// </summary>
        /// <value>The global variables context.</value>
        public IVariablesContext GlobalVariablesContext
        {
            get { throw new NotSupportedException("GlobalVariablesContext not supported for Multilogger"); }
        }


        /// <summary>
        /// Returns the thread-specific context for variables
        /// </summary>
        /// <value>The thread variables context.</value>
        public IVariablesContext ThreadVariablesContext
        {
            get {throw new NotSupportedException("ThreadVariablesContext not supported for MultiLogger");}
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Trace(formatProvider, formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Trace(formatProvider, formatMessageCallback));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Trace(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Trace(formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Trace(Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Trace(formatMessageCallback));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Trace(object message, Exception exception)
        {
            _loggers.ForEach(l => l.Trace(message, exception));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Trace(object message)
        {
            _loggers.ForEach(l => l.Trace(message));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.TraceFormat(formatProvider, format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args"></param>
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            _loggers.ForEach(l => l.TraceFormat(formatProvider, format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void TraceFormat(string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.TraceFormat(format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args">the list of format arguments</param>
        public void TraceFormat(string format, params object[] args)
        {
            _loggers.ForEach(l => l.TraceFormat(format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack Warn.</param>
        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Warn(formatProvider, formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Warn(formatProvider, formatMessageCallback));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Warn(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            _loggers.ForEach(l => l.Warn(formatMessageCallback, exception));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <param name="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        public void Warn(Action<FormatMessageHandler> formatMessageCallback)
        {
            _loggers.ForEach(l => l.Warn(formatMessageCallback));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Warn(object message, Exception exception)
        {
            _loggers.ForEach(l => l.Warn(message, exception));
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Warn(object message)
        {
            _loggers.ForEach(l => l.Warn(message));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.WarnFormat(formatProvider, format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args"></param>
        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            _loggers.ForEach(l => l.WarnFormat(formatProvider, format, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            _loggers.ForEach(l => l.WarnFormat(format, exception, args));
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/></param>
        /// <param name="args">the list of format arguments</param>
        public void WarnFormat(string format, params object[] args)
        {
            _loggers.ForEach(l => l.WarnFormat(format, args));
        }
    }
}
