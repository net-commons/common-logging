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
using System.Collections.Generic;
using System.Configuration;
using Serilog.Events;
using Serilog.Core;
using Serilog.Debugging;

namespace Common.Logging.Serilog
{
    /// <summary>
    /// Routes all log events logged through Serilog into the Common.Logging infrastructure.
    /// </summary>   
    public class CommonLoggingSink : ILogEventSink
    {
        private delegate string MessageFormatter();
        private delegate void LogMethod(ILog logger, MessageFormatter fmtr, Exception exception);
        private static readonly Dictionary<LogEventLevel, LogMethod> logMethods;

        static CommonLoggingSink()
        {
            logMethods = new Dictionary<LogEventLevel, LogMethod>();
            logMethods[LogEventLevel.Verbose] = delegate (ILog log, MessageFormatter msg, Exception ex) { log.Trace(delegate (FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[LogEventLevel.Debug] = delegate (ILog log, MessageFormatter msg, Exception ex) { log.Debug(delegate (FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[LogEventLevel.Information] = delegate (ILog log, MessageFormatter msg, Exception ex) { log.Info(delegate (FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[LogEventLevel.Warning] = delegate (ILog log, MessageFormatter msg, Exception ex) { log.Warn(delegate (FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[LogEventLevel.Error] = delegate (ILog log, MessageFormatter msg, Exception ex) { log.Error(delegate (FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[LogEventLevel.Fatal] = delegate (ILog log, MessageFormatter msg, Exception ex) { log.Fatal(delegate (FormatMessageHandler m) { m(msg()); }, ex); };
        }

        /// <summary>
        /// Creates this target using the default formatProvider.
        /// </summary>
        public CommonLoggingSink()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        public void Emit(LogEvent logEvent)
        {
            try
            {
                if (logEvent == null) throw new ArgumentNullException("logEvent");

                if (LogManager.Adapter is SerilogLoggerFactoryAdapter)
                {
                    throw new ConfigurationErrorsException("routing Serilog events to Common.Logging configured with SerilogLoggerFactoryAdapter results in an endless recursion");
                }

                LogEventPropertyValue type;
                var loggerTypeFullName = GetType().FullName;

                if (logEvent.Properties.TryGetValue("Common.Logging.Type", out type))
                {
                    var sv = type as ScalarValue;
                    if (sv != null && sv.Value is string)
                        loggerTypeFullName = (string)sv.Value;
                }

                ILog logger = LogManager.GetLogger(loggerTypeFullName);
                LogMethod log = logMethods[logEvent.Level];

                log(logger, delegate { return logEvent.RenderMessage(null); }, logEvent.Exception);
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("An Exception Occurred Ex: {0}", ex);
            }            
        }
    }
}