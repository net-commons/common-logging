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
using System.IO;
using Common.Logging.Configuration;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace Common.Logging.Log4Net
{
    /// <summary>
    /// Routes log events to Common.Logging infrastructure.
    /// </summary>
    /// <example>
    /// To route all events logged using log4net to Common.Logging, you need to configure this appender as shown below:
    /// <code>
    /// &lt;log4net&gt;
    ///     &lt;appender name=&quot;CommonLoggingAppender&quot; 
    ///               type=&quot;Common.Logging.Log4Net.CommonLoggingAppender, Common.Logging.Log4Net&quot;&gt;
    ///         &lt;layout type=&quot;log4net.Layout.PatternLayout, log4net&quot;&gt;
    ///             &lt;param name=&quot;ConversionPattern&quot; value=&quot;%level - %class.%method: %message&quot; /&gt;
    ///         &lt;/layout&gt;
    ///     &lt;/appender&gt;
    /// 
    ///     &lt;root&gt;
    ///         &lt;level value=&quot;ALL&quot; /&gt;
    ///         &lt;appender-ref ref=&quot;CommonLoggingAppender&quot; /&gt;
    ///     &lt;/root&gt;
    /// &lt;/log4net&gt;
    /// </code>
    /// </example>
    /// <author>Erich Eichinger</author>
    public class CommonLoggingAppender : AppenderSkeleton
    {
        /// <summary>
        /// Wrapper class that prevents exceptions from being rendered in the message
        /// </summary>
        private class ExceptionAwareLayout : ILayout
        {
            public readonly ILayout InnerLayout;

            public ExceptionAwareLayout(ILayout inner)
            {
                InnerLayout = inner;
            }

            public void Format(TextWriter writer, LoggingEvent loggingEvent)
            {
                InnerLayout.Format(writer, loggingEvent);
            }

            public string ContentType
            {
                get { return InnerLayout.ContentType; }
            }

            public string Header
            {
                get { return InnerLayout.Header; }
            }

            public string Footer
            {
                get { return InnerLayout.Footer; }
            }

            public bool IgnoresException
            {
                get { return false; }
            }
        }

        private delegate string MessageFormatter();
        private delegate void LogMethod(ILog logger, MessageFormatter fmtr, Exception exception);

        private static readonly Dictionary<Level, LogMethod> logMethods;

        static CommonLoggingAppender()
        {
            logMethods = new Dictionary<Level, LogMethod>();
            logMethods[Level.Trace] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Trace(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[Level.Debug] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Debug(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[Level.Info] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Info(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[Level.Warn] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Warn(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[Level.Error] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Error(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[Level.Fatal] = delegate(ILog log, MessageFormatter msg, Exception ex) { log.Fatal(delegate(FormatMessageHandler m) { m(msg()); }, ex); };
            logMethods[Level.All] = logMethods[Level.Trace];
            logMethods[Level.Off] = delegate(ILog log, MessageFormatter msg, Exception ex) { };
        }

        /// <summary>
        /// Gets the closest level supported by Common.Logging of the given log4net level
        /// </summary>
        protected static Level GetClosestLevel(Level currentLevel)
        {
            if (currentLevel.Equals(Level.Off))
                return Level.Off;
            if (currentLevel.Equals(Level.All))
                return Level.All;

            if (currentLevel >= Level.Fatal)
                return Level.Fatal;
            if (currentLevel >= Level.Error)
                return Level.Error;
            if (currentLevel >= Level.Warn)
                return Level.Warn;
            if (currentLevel >= Level.Info)
                return Level.Info;
            if (currentLevel >= Level.Debug)
                return Level.Debug;
            if (currentLevel >= Level.Trace)
                return Level.Trace;

            return Level.All;
        }

        ///<summary>
        /// Get or set the layout for this appender
        ///</summary>
        public override log4net.Layout.ILayout Layout
        {
            get
            {
                return (base.Layout is ExceptionAwareLayout)
                    ? ((ExceptionAwareLayout)base.Layout).InnerLayout
                    : base.Layout;
            }
            set
            {
                ArgUtils.AssertNotNull("Layout", value);
                if (!(value is ExceptionAwareLayout))
                {
                    value = new ExceptionAwareLayout(value);
                }
                base.Layout = value;
            }
        }

        /// <summary>
        /// Sends the given log event to Common.Logging
        /// </summary>
        protected override void Append(LoggingEvent loggingEvent)
        {
            ILog logger = LogManager.GetLogger(loggingEvent.LoggerName);
            Level logLevel = GetClosestLevel(loggingEvent.Level);
            LogMethod log = logMethods[logLevel];
            loggingEvent.Fix = FixFlags.LocationInfo;
            log(logger, delegate { return RenderLoggingEvent(loggingEvent); }, loggingEvent.ExceptionObject);
        }
    }
}