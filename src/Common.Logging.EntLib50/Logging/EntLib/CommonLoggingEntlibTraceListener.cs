#region License

/*
 * Copyright 2002-2009 the original author or authors.
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
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Use this <see cref="TraceListener"/> implementation to route all Entlib logging events to the
    /// Common.Logging infrastructure.
    /// </summary>
    /// <remarks>
    /// See <see cref="CommonLoggingEntlibTraceListenerData"/> for a list of properties to configure.
    /// </remarks>
    /// <example>
    /// To route all <see cref="Logger"/> events to Common.Logging, configure <see cref="CommonLoggingEntlibTraceListener"/>:
    /// <code lang="XML">
    /// &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
    /// &lt;configuration&gt;
    ///     &lt;configSections&gt;
    ///         &lt;section name=&quot;loggingConfiguration&quot; 
    /// type=&quot;Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LoggingSettings, 
    /// Microsoft.Practices.EnterpriseLibrary.Logging, Version=4.1.0.0, Culture=neutral, 
    /// PublicKeyToken=b03f5f7f11d50a3a&quot; /&gt;
    ///     &lt;/configSections&gt;
    ///     &lt;loggingConfiguration name=&quot;Logging Application Block&quot; tracingEnabled=&quot;true&quot;
    ///         defaultCategory=&quot;General&quot; logWarningsWhenNoCategoriesMatch=&quot;true&quot;&gt;
    ///         &lt;listeners&gt;
    ///             &lt;add name=&quot;Common.Logging Listener&quot; 
    ///                  type=&quot;Common.Logging.EntLib.CommonLoggingEntlibTraceListener, Common.Logging.EntLib&quot;
    ///                  listenerDataType=&quot;Common.Logging.EntLib.CommonLoggingEntlibTraceListenerData, 
    /// Common.Logging.EntLib&quot;
    ///                  loggerNameFormat=&quot;{listenerName}.{sourceName}&quot;
    ///                  formatter=&quot;Text Formatter&quot;
    ///                  /&gt;
    ///         &lt;/listeners&gt;
    ///         &lt;formatters&gt;
    ///             &lt;add template=&quot;Timestamp: {timestamp}&amp;#xD;&amp;#xA;Message: {message}&amp;#xD;&amp;#xA;Category: 
    /// {category}&amp;#xD;&amp;#xA;Priority: {priority}&amp;#xD;&amp;#xA;EventId: {eventid}&amp;#xD;&amp;#xA;Severity: 
    /// {severity}&amp;#xD;&amp;#xA;Title:{title}&amp;#xD;&amp;#xA;Machine: {machine}&amp;#xD;&amp;#xA;Application Domain: 
    /// {appDomain}&amp;#xD;&amp;#xA;Process Id: {processId}&amp;#xD;&amp;#xA;Process Name: {processName}&amp;#xD;&amp;#xA;Win32 
    /// Thread Id: {win32ThreadId}&amp;#xD;&amp;#xA;Thread Name: {threadName}&amp;#xD;&amp;#xA;Extended Properties: 
    /// {dictionary({key} - {value}&amp;#xD;&amp;#xA;)}&quot;
    ///                 type=&quot;Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.TextFormatter, 
    /// Microsoft.Practices.EnterpriseLibrary.Logging, Version=4.1.0.0, Culture=neutral, 
    /// PublicKeyToken=b03f5f7f11d50a3a&quot;
    ///                 name=&quot;Text Formatter&quot; /&gt;
    ///         &lt;/formatters&gt;
    ///         &lt;specialSources&gt;
    ///             &lt;allEvents switchValue=&quot;All&quot; name=&quot;All Events&quot;&gt;
    ///                 &lt;listeners&gt;
    ///                     &lt;add name=&quot;Test Capturing Listener&quot; /&gt;
    ///                 &lt;/listeners&gt;
    ///             &lt;/allEvents&gt;
    ///         &lt;/specialSources&gt;
    ///     &lt;/loggingConfiguration&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// <author>Erich Eichinger</author>
    public class CommonLoggingEntlibTraceListener : FormattedTraceListenerBase
    {
        /// <summary>
        /// The message object to be logged. Overrides <see cref="ToString"/> to format 
        /// the associated <see cref="LogEntry"/>
        /// </summary>
        private class LogEntryMessage
        {
            private readonly ILogFormatter _logFormatter;
            private readonly LogEntry _logEntry;
            private string _cachedResult;

            public LogEntryMessage(ILogFormatter logFormatter, LogEntry logEntry)
            {
                _logFormatter = logFormatter;
                _logEntry = logEntry;
            }

            public override string ToString()
            {
                if (_cachedResult == null)
                {
                    if (_logFormatter == null)
                    {
                        _cachedResult = _logEntry.ToString();
                    }
                    else
                    {
                        _cachedResult = _logFormatter.Format(_logEntry);
                    }
                }
                return _cachedResult;
            }
        }

        // used to format the loggername from listener + source names
        private readonly string _loggerNameFormat = "{listenerName}.{sourceName}";
        private string _loggerName;

        /// <summary>
        /// Initializes this instance from <see cref="CommonLoggingEntlibTraceListenerData"/> configuration
        /// information.
        /// </summary>
        public CommonLoggingEntlibTraceListener(CommonLoggingEntlibTraceListenerData data, ILogFormatter formatter)
            :base(formatter)
        {
            if (data.LoggerNameFormat != null)
            {
                _loggerNameFormat = data.LoggerNameFormat;
            }
            _loggerName = data.Name;
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
        }

        /// <summary>
        /// NOT USED BY ENTLIB
        /// </summary>
        /// <exception cref="NotImplementedException"/>
        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT USED BY ENTLIB
        /// </summary>
        /// <exception cref="NotImplementedException"/>
        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overridden to redirect to call <see cref="Log"/>.
        /// </summary>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    data = new LogEntryMessage(base.Formatter, (LogEntry)data);
                }
                Log(eventType, source, id, "{0}", data);
            }
        }

        /// <summary>
        /// Logs the given message to the Common.Logging infrastructure
        /// </summary>
        protected virtual void Log(TraceEventType eventType, string source, int id, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = this.LoggerNameFormat.Replace("{listenerName}", _loggerName).Replace("{sourceName}", source);
            }

            ILog log = LogManager.GetLogger(source);
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