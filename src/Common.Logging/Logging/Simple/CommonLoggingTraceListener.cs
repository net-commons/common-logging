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
using System.Text;

namespace Common.Logging.Simple
{
    /// <summary>
    /// A <see cref="TraceListener"/> implementation sending all <see cref="Trace">System.Diagnostics.Trace</see> output to 
    /// the Common.Logging infrastructure.
    /// </summary>
    /// <remarks>
    /// This listener captures all output sent by calls to <see cref="System.Diagnostics.Trace">System.Diagnostics.Trace</see> and
    /// and <see cref="TraceSource"/> and sends it to an <see cref="ILog"/> instance.<br/>
    /// The <see cref="ILog"/> instance to be used is obtained by calling
    /// <see cref="LogManager.GetLogger(string)"/>. The name of the logger is created by passing 
    /// this listener's <see cref="TraceListener.Name"/> and any <c>source</c> or <c>category</c> passed 
    /// into this listener (see <see cref="TraceListener.WriteLine(object,string)"/> or <see cref="TraceListener.TraceEvent(TraceEventCache,string,TraceEventType,int,string,object[])"/> for example).
    /// </remarks>
    /// <example>
    /// The snippet below shows how to add and configure this listener to your app.config:
    /// <code lang="XML">
    /// &lt;system.diagnostics&gt;
    ///   &lt;sharedListeners&gt;
    ///     &lt;add name=&quot;Diagnostics&quot;
    ///          type=&quot;Common.Logging.Simple.CommonLoggingTraceListener, Common.Logging&quot;
    ///          initializeData=&quot;DefaultTraceEventType=Information; LoggerNameFormat={listenerName}.{sourceName}&quot;&gt;
    ///       &lt;filter type=&quot;System.Diagnostics.EventTypeFilter&quot; initializeData=&quot;Information&quot;/&gt;
    ///     &lt;/add&gt;
    ///   &lt;/sharedListeners&gt;
    ///   &lt;trace&gt;
    ///     &lt;listeners&gt;
    ///       &lt;add name=&quot;Diagnostics&quot; /&gt;
    ///     &lt;/listeners&gt;
    ///   &lt;/trace&gt;
    /// &lt;/system.diagnostics&gt;
    /// </code>
    /// </example>
    /// <author>Erich Eichinger</author>
    public class CommonLoggingTraceListener : TraceListener
    {
        private TraceEventType _defaultTraceEventType = TraceEventType.Verbose;
        private string _loggerNameFormat = "{listenerName}.{sourceName}";
        private int _callDepth;

        #region Properties

        /// <summary>
        /// Sets the default <see cref="TraceEventType"/> to use for logging
        /// all events emitted by <see cref="Trace"/><c>.Write(...)</c> and
        /// <see cref="Trace"/><c>.WriteLine(...)</c> methods.
        /// </summary>
        /// <remarks>
        /// This listener captures all output sent by calls to <see cref="System.Diagnostics.Trace"/> and
        /// sends it to an <see cref="ILog"/> instance using the <see cref="Common.Logging.LogLevel"/> specified
        /// on <see cref="LogLevel"/>.
        /// </remarks>
        public TraceEventType DefaultTraceEventType
        {
            get { return _defaultTraceEventType; }
            set { _defaultTraceEventType = value; }
        }

        /// <summary>
        /// Format to use for creating the logger name. Defaults to "{listenerName}.{sourceName}".
        /// </summary>
        /// <remarks>
        /// Available placeholders are:
        /// <list type="bullet">
        /// <item>{listenerName}: the configured name of this listener instance.</item>
        /// <item>{sourceName}: the trace source name an event originates from (see e.g. <see cref="TraceListener.TraceEvent(System.Diagnostics.TraceEventCache,string,System.Diagnostics.TraceEventType,int,string,object[])"/>.</item>
        /// </list>
        /// </remarks>
        public string LoggerNameFormat
        {
            get { return _loggerNameFormat; }
            set { _loggerNameFormat = value; }
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
            if (props["defaultTraceEventType"] != null)
            {
                this._defaultTraceEventType = (TraceEventType)Enum.Parse(typeof(TraceEventType), props["defaultTraceEventType"], true);
            }
            else
            {
                this._defaultTraceEventType = TraceEventType.Verbose;
            }

            if (props["name"] != null)
            {
                this.Name = props["name"];
            }
            else
            {
                this.Name = "Diagnostics";
            }

            if (props["loggerNameFormat"] != null)
            {
                this.LoggerNameFormat = props["loggerNameFormat"];
            }
            else
            {
                this.LoggerNameFormat = "{listenerName}.{sourceName}";
            }
        }

        /// <summary>
        /// Logs the given message to the Common.Logging infrastructure.
        /// </summary>
        /// <param name="eventType">the eventType</param>
        /// <param name="source">the <see cref="TraceSource"/> name or category name passed into e.g. <see cref="Trace.Write(object,string)"/>.</param>
        /// <param name="id">the id of this event</param>
        /// <param name="format">the message format</param>
        /// <param name="args">the message arguments</param>
        protected virtual void Log(TraceEventType eventType, string source, int id, string format, params object[] args)
        {
            source = this.LoggerNameFormat.Replace("{listenerName}", this.Name).Replace("{sourceName}", ""+source);


            //ensure that Log(...) isn't called recursively
            // necessary b/c Log4Net calls Trace.Write(...) during its initialization this otherwise results in a StackOverflow exception 
            // (see https://github.com/net-commons/common-logging/issues/127 for details)
            _callDepth++;

            if (_callDepth > 1)
                return;

            ILog log = LogManager.GetLogger(source);

            _callDepth--;

            LogLevel logLevel = MapLogLevel(eventType);

            switch (logLevel)
            {
                case LogLevel.Trace:
                    log.TraceFormat(format, args);
                    break;
                case LogLevel.Debug:
                    log.DebugFormat(format, args);
                    break;
                case LogLevel.Info:
                    log.InfoFormat(format, args);
                    break;
                case LogLevel.Warn:
                    log.WarnFormat(format, args);
                    break;
                case LogLevel.Error:
                    log.ErrorFormat(format, args);
                    break;
                case LogLevel.Fatal:
                    log.FatalFormat(format, args);
                    break;
                case LogLevel.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eventType", eventType, "invalid TraceEventType value");
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
                if (part.Length == 0)
                    continue;

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
        public override void Write(object o)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, "{0}", o);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(object o, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, "{0}", o);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(string message)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, message);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(string message, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, message);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(object o)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, "{0}", o);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(object o, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, "{0}", o);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(string message)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, message);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void WriteLine(string message, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, message);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
            {
                Log(eventType, source, id, "Event Id {0}", id);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
            {
                Log(eventType, source, id, message);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message, params object[] args)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, args, null, null))
            {
                Log(eventType, source, id, message, args);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
            {
                string fmt = GetFormat((object[])data);
                Log(eventType, source, id, fmt, data);
            }
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                string fmt = GetFormat((object)data);
                Log(eventType, source, id, fmt, data);
            }
        }

        private string GetFormat(params object[] data)
        {
            if (data == null || data.Length == 0)
                return null;
            StringBuilder fmt = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                fmt.Append('{').Append(i).Append('}');
                if (i < data.Length - 1)
                    fmt.Append(',');
            }
            return fmt.ToString();
        }

        private LogLevel MapLogLevel(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    return LogLevel.Trace;
                case TraceEventType.Verbose:
                    return LogLevel.Debug;
                case TraceEventType.Information:
                    return LogLevel.Info;
                case TraceEventType.Warning:
                    return LogLevel.Warn;
                case TraceEventType.Error:
                    return LogLevel.Error;
                case TraceEventType.Critical:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Trace;
            }
        }
    }
}