#region License

/*
 * Copyright © 2002-2015 the original author or authors.
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

namespace Common.Logging.ApplicationInsights
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using System;
    using System.Text;
    using System.Collections.Generic;

    /// <summary>
    /// Sends log messages to Application Insights.
    /// </summary>
    /// <author>Mihail Smacinih</author>
    public class ApplicationInsightsLogger : Simple.AbstractSimpleLogger
    {
        private static readonly List<KeyValuePair<LogLevel, SeverityLevel>> levelMap = new List<KeyValuePair<LogLevel, SeverityLevel>>();

        static ApplicationInsightsLogger()
        {
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.Fatal, SeverityLevel.Critical));
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.Error, SeverityLevel.Error));
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.Warn, SeverityLevel.Warning));
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.Info, SeverityLevel.Information));
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.Debug, SeverityLevel.Verbose));
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.Trace, SeverityLevel.Verbose));
            levelMap.Add(Create<LogLevel, SeverityLevel>(LogLevel.All, SeverityLevel.Verbose));
        }

        /// <summary>
        /// The telemetry client
        /// </summary>
        public readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsLogger"/> class.
        /// </summary>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        /// <param name="logName">Name of the log.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="showlevel">if set to <c>true</c> [showlevel].</param>
        /// <param name="showDateTime">if set to <c>true</c> [show date time].</param>
        /// <param name="showLogName">if set to <c>true</c> [show log name].</param>
        /// <param name="dateTimeFormat">The date time format.</param>
        public ApplicationInsightsLogger(string instrumentationKey, string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
        {
            this.telemetryClient = new TelemetryClient();
            this.telemetryClient.Context.InstrumentationKey = instrumentationKey;
        }

        /// <summary>
        /// Actually sends the message to the underlying log system.
        /// </summary>
        /// <param name="level">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            if (level.HasFlag(LogLevel.Off)) 
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            FormatOutput(sb, level, message, exception);

            SeverityLevel severityLevel = ConvertToSeverityLevel(level);
            this.telemetryClient.TrackTrace(sb.ToString(), severityLevel);

            if (exception != null)
            {
                this.telemetryClient.TrackException(exception);
            }
        }

        private static SeverityLevel ConvertToSeverityLevel(LogLevel logLevel)
        {
            SeverityLevel result = SeverityLevel.Verbose;
            foreach (var kv in levelMap) 
            {
                if (logLevel.HasFlag(kv.Key)) 
                {
                    result = kv.Value;
                    break;
                }
            }

            return result;
        }

        private static KeyValuePair<K, V> Create<K, V>(K key, V value)
        {
            return new KeyValuePair<K, V>(key, value);
        }
    }
}
