#region License

/*
 * Copyright � 2002-2009 the original author or authors.
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

namespace Common.Logging
{
    /// <summary>
	/// A simple logging interface abstracting logging APIs. 
	/// </summary>
    /// <remarks>
    /// <para>
    /// Implementations should defer calling a message's <see cref="object.ToString()"/> until the message really needs
    /// to be logged to avoid performance penalties.
    /// </para>
    /// <para>
    /// Each <see cref="ILog"/> log method offers to pass in a <see cref="FormatMessageCallback"/> instead of the actual message.
    /// Using this style has the advantage to defer possibly expensive message argument evaluation and formatting (and formatting arguments!) until the message gets
    /// actually logged. If the message is not logged at all (e.g. due to <see cref="LogLevel"/> settings), 
    /// you won't have to pay the peformance penalty of creating the message.
    /// </para>
    /// </remarks>
    /// <example>
    /// The example below demonstrates using callback style for creating the message, where the call to the 
    /// <see cref="Random.NextDouble"/> and the underlying <see cref="string.Format(string,object[])"/> only happens, if level <see cref="LogLevel.Debug"/> is enabled:
    /// <code>
    /// Log.Debug( m=&gt;m(&quot;result is {0}&quot;, random.NextDouble()) );
    /// Log.Debug(delegate(m) { m(&quot;result is {0}&quot;, random.NextDouble()); });
    /// </code>
    /// </example>
    /// <seealso cref="FormatMessageCallback"/>
    /// <author>Mark Pollack</author>
    /// <author>Bruno Baia</author>
    /// <author>Erich Eichinger</author>
    public interface ILog
	{
        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        void Trace(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Trace"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Trace(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        void TraceFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        void TraceFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        void TraceFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Trace(FormatMessageCallback formatMessageCallback);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Trace(FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Trace"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Debug"/> level.
		/// </summary>
		/// <param key="message">The message object to log.</param>
		void Debug( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Debug"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param key="message">The message object to log.</param>
		/// <param key="exception">The exception to log, including its stack trace.</param>
		void Debug( object message, Exception exception );

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        void DebugFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        void DebugFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Debug(FormatMessageCallback formatMessageCallback);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Debug(FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        void Info(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        void InfoFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        void InfoFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Info(FormatMessageCallback formatMessageCallback);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Info(FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        void Warn(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param key="message">The message object to log.</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        void WarnFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        void WarnFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Warn(FormatMessageCallback formatMessageCallback);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Warn(FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
		/// Log a message object with the <see cref="LogLevel.Error"/> level.
		/// </summary>
		/// <param key="message">The message object to log.</param>
		void Error( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Error"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param key="message">The message object to log.</param>
		/// <param key="exception">The exception to log, including its stack trace.</param>
		void Error( object message, Exception exception );

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        void ErrorFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Error(FormatMessageCallback formatMessageCallback);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Error(FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
		/// Log a message object with the <see cref="LogLevel.Fatal"/> level.
		/// </summary>
		/// <param key="message">The message object to log.</param>
		void Fatal( object message );

		/// <summary>
		/// Log a message object with the <see cref="LogLevel.Fatal"/> level including
		/// the stack trace of the <see cref="Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param key="message">The message object to log.</param>
		/// <param key="exception">The exception to log, including its stack trace.</param>
		void Fatal( object message, Exception exception );

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args">the list of format arguments</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args">the list of format arguments</param>
        void FatalFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="args"></param>
        void FatalFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param key="exception">The exception to log.</param>
        /// <param key="args"></param>
        void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Fatal(FormatMessageCallback formatMessageCallback);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        /// <param key="exception">The exception to log, including its stack trace.</param>
        void Fatal(FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level using a callback to obtain the message
        /// </summary>
        /// <remarks>
        /// Using this method avoids the cost of creating a message and evaluating message arguments 
        /// that probably won't be logged due to loglevel settings.
        /// </remarks>
        /// <param key="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param key="formatMessageCallback">A callback used by the logger to obtain the message if log level is matched</param>
        void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback);

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
        void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception);

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Trace"/> level.
        /// </summary>
        bool IsTraceEnabled
        {
            get;
        }

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
