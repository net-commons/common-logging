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
using Common.Logging.Factory;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to Enterprise Logging 3.1.
    /// </summary>
    /// <remarks>
    /// Instances are created by the <see cref="EntLibLoggerFactoryAdapter"/>. <see cref="EntLibLoggerFactoryAdapter.DefaultPriority"/>
    /// is used for logging a <see cref="LogEntry"/> to <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter.Write"/>.
    /// The category name used is the name passed into <see cref="LogManager.GetLogger(string)" />. For configuring logging, see <see cref="EntLibLoggerFactoryAdapter"/>.
    /// </remarks>
    /// <seealso cref="ILog"/>
    /// <seealso cref="EntLibLoggerFactoryAdapter"/>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    public class EntLibLogger  : AbstractLogger
    {
        private class TraceLevelLogEntry : LogEntry
        {
            public TraceLevelLogEntry(string category, TraceEventType severity)
            {
                Categories.Add(category);
                Severity = severity;               
            }
        }

        private readonly LogEntry VerboseLogEntry;
        private readonly LogEntry InformationLogEntry;
        private readonly LogEntry WarningLogEntry;
        private readonly LogEntry ErrorLogEntry;
        private readonly LogEntry CriticalLogEntry;

        private readonly string category;
        private readonly EntLibLoggerSettings settings;
        private readonly LogWriter logWriter;

        /// <summary>
        /// The category of this logger
        /// </summary>
        public string Category
        {
            get { return category; }
        }

        /// <summary>
        /// The settings used by this logger
        /// </summary>
        public EntLibLoggerSettings Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// The <see cref="LogWriter"/> used by this logger.
        /// </summary>
        public LogWriter LogWriter
        {
            get { return logWriter; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLogger"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="logWriter">the <see cref="LogWriter"/> to write log events to.</param>
        /// <param name="settings">the logger settings</param>
        public EntLibLogger(string category, LogWriter logWriter, EntLibLoggerSettings settings)
        {
            this.category = category;
            this.logWriter = logWriter;
            this.settings = settings;

            VerboseLogEntry = new TraceLevelLogEntry(category, TraceEventType.Verbose);
            InformationLogEntry = new TraceLevelLogEntry(category, TraceEventType.Information);
            WarningLogEntry = new TraceLevelLogEntry(category, TraceEventType.Warning);
            ErrorLogEntry = new TraceLevelLogEntry(category, TraceEventType.Error);
            CriticalLogEntry = new TraceLevelLogEntry(category, TraceEventType.Critical);
        }

        #region IsXXXXEnabled

        /// <summary>
        /// Gets a value indicating whether this instance is trace enabled.  
        /// </summary>
        public override bool IsTraceEnabled
        {
            get { return ShouldLog(VerboseLogEntry); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled. 
        /// </summary>
        public override bool IsDebugEnabled
        {
            get { return ShouldLog(VerboseLogEntry); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        public override bool IsInfoEnabled
        {
            get { return ShouldLog(InformationLogEntry); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        public override bool IsWarnEnabled
        {
            get { return ShouldLog(WarningLogEntry); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        public override bool IsErrorEnabled
        {
            get { return ShouldLog(ErrorLogEntry); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        public override bool IsFatalEnabled
        {
            get { return ShouldLog(CriticalLogEntry); }
        }


        #endregion
        
        /// <summary>
        /// Actually sends the message to the EnterpriseLogging log system.
        /// </summary>
        /// <param name="logLevel">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        protected override void WriteInternal(LogLevel logLevel, object message, Exception exception)
        {
            LogEntry log = CreateLogEntry(GetTraceEventType(logLevel));

            if (ShouldLog(log))
            {
                PopulateLogEntry(log, message, exception);
                WriteLog(log);
            }
        }

        /// <summary>
        /// May be overridden for custom filter logic
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        protected virtual bool ShouldLog(LogEntry log)
        {
            return logWriter.ShouldLog(log);
        }

        /// <summary>
        /// Write the fully populated event to the log.
        /// </summary>
        protected virtual void WriteLog(LogEntry log)
        {
            logWriter.Write(log);
        }

        /// <summary>
        /// Translates a <see cref="LogLevel"/> to a <see cref="TraceEventType"/>.
        /// </summary>
        protected virtual TraceEventType GetTraceEventType(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return TraceEventType.Verbose;
                case LogLevel.Trace:
                    return TraceEventType.Verbose;
                case LogLevel.Debug:
                    return TraceEventType.Verbose;
                case LogLevel.Info:
                    return TraceEventType.Information;
                case LogLevel.Warn:
                    return TraceEventType.Warning;
                case LogLevel.Error:
                    return TraceEventType.Error;
                case LogLevel.Fatal:
                    return TraceEventType.Critical;
                case LogLevel.Off:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, "unknown log level");
            }
        }

        /// <summary>
        /// Creates a minimal log entry instance that will be passed into <see cref="Logger.ShouldLog"/>
        /// to asap decide, whether this event should be logged.
        /// </summary>
        /// <param name="traceEventType">trace event severity.</param>
        /// <returns></returns>
        protected virtual LogEntry CreateLogEntry(TraceEventType traceEventType)
        {
            LogEntry log = new LogEntry();
            log.Categories.Add(category);
            log.Priority = settings.priority;
            log.Severity = traceEventType;
            return log;
        }

        /// <summary>
        /// Configures the log entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        protected virtual void PopulateLogEntry(LogEntry log, object message, Exception ex)
        {
            log.Message = (message == null ? null : message.ToString());
            if (ex != null)
            {
                AddExceptionInfo(log, ex);
            }
        }

        /// <summary>
        /// Adds the exception info.
        /// </summary>
        /// <param name="log">The log entry.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        protected virtual void AddExceptionInfo(LogEntry log, Exception exception)
        {
            if (exception != null && settings.exceptionFormat != null)
            {
                string errorMessage = settings.exceptionFormat
                    .Replace("$(exception.message)", exception.Message)
                    .Replace("$(exception.source)", exception.Source)
                    .Replace("$(exception.targetsite)", (exception.TargetSite==null)?string.Empty:exception.TargetSite.ToString())
                    .Replace("$(exception.stacktrace)", exception.StackTrace)
                    ;
                //                StringBuilder sb = new StringBuilder(128);
                //                sb.Append("Exception[ ");
                //                sb.Append("message = ").Append(exception.Message).Append(separator);
                //                sb.Append("source = ").Append(exception.Source).Append(separator);
                //                sb.Append("targetsite = ").Append(exception.TargetSite).Append(separator);
                //                sb.Append("stacktrace = ").Append(exception.StackTrace).Append("]");
                //                return sb.ToString();
                log.AddErrorMessage(errorMessage);
            }
        }
    }
}