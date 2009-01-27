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
using System.Collections.Specialized;
using System.Diagnostics;

namespace Common.Logging
{
    /// <summary>
    /// TraceListener sending all trace output to Common.Logging infrastructure.
    /// </summary>
	/// <remarks>
	/// This listener captures all output sent by calls to <see cref="System.Diagnostics.Trace">System.Diagnostics.Trace</see> and
	/// sends it to an <see cref="ILog"/> instance using the log level specified
	/// on <see cref="LogLevel"/>.
	/// The <see cref="ILog"/> instance to be used is obtained by calling
	/// <see cref="LogManager.GetLogger(string)"/>, using this listener's <see cref="Name"/> as the argument.
	/// </remarks>
	/// <example>
	/// The snippet below shows how to add this listener to your app.config:
	/// <code>
    /// &lt;configuration&gt;
    ///   &lt;system.diagnostics&gt;
    ///     &lt;trace&gt;
    ///     &lt;listeners&gt;
    ///       &lt;clear /&gt;
    ///       &lt;add name=&quot;myDiagnostics&quot;
    ///            type=&quot;Common.Logging.CommonLoggingTraceListener, Common.Logging&quot;
    ///            initializeData=&quot;LogLevel=Trace&quot; /&gt;
    ///     &lt;/listeners&gt;
    ///     &lt;/trace&gt;
    ///   &lt;/system.diagnostics&gt;
    /// &lt;/configuration&gt;
	/// </code>
	/// </example>
	/// <author>Erich Eichinger</author>
    public sealed class CommonLoggingTraceListener : TraceListener
    {
        private delegate void LogHandler(object message);

        private LogLevel _logLevel = Logging.LogLevel.Trace;
        private LogHandler _log;

        #region Properties

		/// <summary>
		/// Sets the <see cref="Common.Logging.LogLevel"/> to use for logging
		/// all events captured by this listener.
		/// </summary>
		/// <remarks>
		/// This listener captures all output sent by calls to <see cref="System.Diagnostics.Trace"/> and
		/// sends it to an <see cref="ILog"/> instance using the <see cref="Common.Logging.LogLevel"/> specified
		/// on <see cref="LogLevel"/>.
		/// </remarks>
        public LogLevel LogLevel
        {
            get { return _logLevel; }
            set
            {
                _logLevel = value;
                RefreshLogger();
            }
        }

		/// <summary>
		/// The Name of this <see cref="TraceListener"/>. This name is also used
		/// for obtaining the underlying logger using <see cref="LogManager.GetLogger(string)"/>.
		/// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                RefreshLogger();
            }
        }

        #endregion

        #region Construction

		/// <summary>
		/// Creates a new instance with the default name "Diagnostics" and <see cref="LogLevel"/> "Trace".
		/// </summary>
        public CommonLoggingTraceListener()
            : this(string.Empty)
        { }

		/// <summary>
		/// Creates a new instance initialized with properties from the <paramref name="initializeData"/>. string.
		/// </summary>
		/// <remarks>
		/// <paramref name="initializeData"/> is a semicolon separated string of name/value pairs, where each pair has
		/// the form <c>key=value</c>. E.g.
		/// "<c>Name=MyLoggerName;LogLevel=Debug</c>"
		/// </remarks>
		/// <param name="initializeData">a semicolon separated list of name/value pairs.</param>
        public CommonLoggingTraceListener(string initializeData)
            : this(GetPropertiesFromInitString(initializeData))
        {
        }

        /// <summary>
        /// Creates a new instance initialized with the specified properties.
        /// </summary>
        /// <param name="properties">name/value configuration properties.</param>
        public CommonLoggingTraceListener(NameValueCollection properties)
            : base()
        {
            if (properties == null)
            {
                properties = new NameValueCollection();
            }
            ApplyProperties(properties);
        }

        private void ApplyProperties(NameValueCollection props)
        {
            if (props["logLevel"] != null)
            {
                this._logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), props["logLevel"], true);
            }
            else
            {
                this._logLevel = LogLevel.Trace;
            }

            if (props["name"] != null)
            {
                this.Name = props["name"];
            }
            else
            {
                this.Name = "Diagnostics";
            }
        }

        /// <summary>
        /// Obtains a new logger instance from <see cref="LogManager"/>
        /// based on the current values of <see cref="Name"/> and <see cref="LogLevel"/>.
        /// </summary>
        private void RefreshLogger()
        {
            ILog log = LogManager.GetLogger(this.Name);

            switch (LogLevel)
            {
                case LogLevel.All:
                    _log = new LogHandler(log.Trace); break;
                case LogLevel.Trace:
                    _log = new LogHandler(log.Trace); break;
                case LogLevel.Debug:
                    _log = new LogHandler(log.Debug); break;
                case LogLevel.Info:
                    _log = new LogHandler(log.Info); break;
                case LogLevel.Warn:
                    _log = new LogHandler(log.Warn); break;
                case LogLevel.Error:
                    _log = new LogHandler(log.Error); break;
                case LogLevel.Fatal:
                    _log = new LogHandler(log.Fatal); break;
                case LogLevel.Off:
                    _log = new LogHandler(LogIgnore); break;
                default:
                    throw new ArgumentOutOfRangeException("LogLevel", LogLevel, "unknown log level");
            }
        }

        private static NameValueCollection GetPropertiesFromInitString(string initializeData)
        {
            NameValueCollection props = new NameValueCollection();

            if (initializeData == null)
            {
                return props;
            }

            string[] parts = initializeData.Split(';');
            foreach (string s in parts)
            {
                string part = s.Trim();
                if (part.Length == 0) continue;

                int ixEquals = part.IndexOf('=');
                if (ixEquals > -1)
                {
                    string name = part.Substring(0, ixEquals).Trim();
                    string value = (ixEquals < part.Length - 1) ? part.Substring(ixEquals + 1) : string.Empty;
                    props[name] = value.Trim();
                }
                else
                {
                    props[part.Trim()] = null;
                }
            }
            return props;
        }

        #endregion

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(string message)
        {
            if (_logLevel != LogLevel.Off)
            {
                _log(message);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(string message)
        {
            if (_logLevel != LogLevel.Off)
            {
                _log(message);
            }
        }

        /// <summary>
        /// NoOp Log Method
        /// </summary>
        private static void LogIgnore(object message)
        {
        }
    }
}
