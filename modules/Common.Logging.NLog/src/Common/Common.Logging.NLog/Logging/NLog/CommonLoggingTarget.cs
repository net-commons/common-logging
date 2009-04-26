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
using Common.Logging.Configuration;
using NLog;

namespace Common.Logging.NLog
{
    /// <summary>
    /// Routes all log events logged through NLog into the Common.Logging infrastructure.
    /// </summary>
    /// <remarks>
    /// <example>
    /// To route all NLog events to Common.Logging, you must add this target to your configuration:
    /// <code>
    /// LoggingConfiguration cfg = new LoggingConfiguration();
    /// CommonLoggingTarget target = new CommonLoggingTarget(&quot;${level:uppercase=true}|${logger}|${message}&quot;);
    /// cfg.LoggingRules.Add(new LoggingRule(&quot;*&quot;, LogLevel.Trace, target));
    /// 
    /// LogManager.Configuration = cfg;
    /// 
    /// Logger log = LogManager.GetLogger(&quot;mylogger&quot;);
    /// log.Debug(&quot;some message&quot;);
    /// </code>
    /// </example>
    /// </remarks>
    /// <author>Erich Eichinger</author>
    public class CommonLoggingTarget : TargetWithLayout
    {
        private delegate string MessageFormatter();
        private delegate void LogMethod(ILog logger, MessageFormatter fmtr, Exception exception);

        private static readonly Dictionary<global::NLog.LogLevel, LogMethod> logMethods;

        static CommonLoggingTarget()
        {
            logMethods = new Dictionary<global::NLog.LogLevel, LogMethod>();
            logMethods[global::NLog.LogLevel.Trace] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Trace(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[global::NLog.LogLevel.Debug] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Debug(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[global::NLog.LogLevel.Info] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Info(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[global::NLog.LogLevel.Warn] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Warn(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[global::NLog.LogLevel.Error] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Error(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[global::NLog.LogLevel.Fatal] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Fatal(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[global::NLog.LogLevel.Off] = delegate(ILog log, MessageFormatter msg, Exception ex) { };
        }

        /// <summary>
        /// Creates this target using the default layout.
        /// </summary>
        public CommonLoggingTarget()
        {
        }

        /// <summary>
        /// Creates this target using a custom layout.
        /// </summary>
        public CommonLoggingTarget(string layout)
        {
            ArgUtils.AssertNotNull("layout", layout);
            this.Layout = layout;
        }

        /// <summary>
        /// Writes the event to the Common.Logging infrastructure
        /// </summary>
        protected override void Write(LogEventInfo logEvent)
        {
            ILog logger = LogManager.GetLogger(logEvent.LoggerName);
            LogMethod log = logMethods[logEvent.Level];
            log(logger, delegate { return this.CompiledLayout.GetFormattedMessage(logEvent); }, logEvent.Exception);
        }
    }
}