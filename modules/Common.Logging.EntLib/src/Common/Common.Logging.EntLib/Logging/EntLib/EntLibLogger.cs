#region License

/*
 * Copyright 2002-2007 the original author or authors.
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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to Enterprise Logging.
    /// </summary>
    /// <remarks>
    /// Default priority is used.  The category name used is the name passed into 
    /// LogManger.GetLogger
    /// </remarks>
    /// <author>Mark Pollack</author>
    /// <version>$Id:$</version>
    public class EntLibLogger  : ILog
    {
        private string category;
        
        //TODO these will be configurable in a future version
        //format like nlog is good - i.e. ${exception:format=message,stacktrace:separator=, }
        private int priority = -1;
        private string separator = ", ";
        

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLogger"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public EntLibLogger(string category)
        {
            this.category = category;
        }

        /// <summary>
        /// Logs the specified message at the TraceEventType.Verbose level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(object message)
        {
            WriteLog(TraceEventType.Verbose, message);
        }

        /// <summary>
        /// Logs the specified message and exception at the TraceEventType.Verbose level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Trace(object message, Exception exception)
        {
            WriteLog(TraceEventType.Verbose, message, exception);
        }

        /// <summary>
        /// Logs the specified message at the TraceEventType.Verbose level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
        {            
            WriteLog(TraceEventType.Verbose, message);
        }

        /// <summary>
        /// Logs the specified message and excpetion at the TraceEventType.Verbose level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Debug(object message, Exception exception)
        {
            WriteLog(TraceEventType.Verbose, message, exception);
        }

        /// <summary>
        /// Logs the specified message at the TraceEventType.Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            WriteLog(TraceEventType.Error, message);
        }

        /// <summary>
        /// Logs the specified message and excpetion at the TraceEventType.Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(object message, Exception exception)
        {
            WriteLog(TraceEventType.Error, message, exception);
        }

        /// <summary>
        /// Logs the specified message at the TraceEventType.Critical level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            WriteLog(TraceEventType.Critical, message);
        }

        /// <summary>
        /// Logs the specified message and excpetion at the TraceEventType.Critical level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(object message, Exception exception)
        {
            WriteLog(TraceEventType.Critical, message, exception);
        }

        /// <summary>
        /// Logs the specified message at the TraceEventType.Information level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            WriteLog(TraceEventType.Information, message);
        }

        /// <summary>
        /// Logs the specified message and excpetion at the TraceEventType.Information level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Info(object message, Exception exception)
        {
            WriteLog(TraceEventType.Information, message, exception);
        }

        /// <summary>
        /// Logs the specified message at the TraceEventType.Warning level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            WriteLog(TraceEventType.Warning, message);
        }

        /// <summary>
        /// Logs the specified message and excpetion at the TraceEventType.Warning level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warn(object message, Exception exception)
        {
            WriteLog(TraceEventType.Warning, message, exception);
        }

        // Needs investigation.  At the moment it doesn't seem possible to check
        // for 'ShouldLog' based on log level.

        /// <summary>
        /// Gets a value indicating whether this instance is trace enabled.  
        /// </summary>
        /// <value>
        /// 	<c>true</c> always.
        /// </value>
        public bool IsTraceEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled. 
        /// </summary>
        /// <value>
        /// 	<c>true</c> always
        /// </value>
        public bool IsDebugEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> always.
        /// </value>
        public bool IsErrorEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> always
        /// </value>
        public bool IsFatalEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> always
        /// </value>
        public bool IsInfoEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> always
        /// </value>
        public bool IsWarnEnabled
        {
            get { return true; }
        }


        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="traceEventType">Type of the trace event.</param>
        /// <param name="message">The message.</param>
        protected virtual void WriteLog(TraceEventType traceEventType, object message)
        {
            WriteLog(traceEventType, message, null);
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="traceEventType">Type of the trace event.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        protected virtual void WriteLog(TraceEventType traceEventType, object message, Exception ex)
        {
            LogEntry log = CreateLogEntry(traceEventType);
            
            // 'Logger.ShouldLog' type functionality should go here, but it doesn't take into account levels.
            // One apporach is to have execution of ShouldLog part of the configuration.  
            // In anycase, the extra info collected isn't very expensive to generate.   
         
            if (ex != null)
            {
                ConfigureLogEntry(log, message, ex);
            }
            else
            {
                ConfigureLogEntry(log, message);
            }            
            Logger.Write(log);
        }

        /// <summary>
        /// Creates the log entry.
        /// </summary>
        /// <param name="traceEventType">Type of the trace event.</param>
        /// <returns></returns>
        protected virtual LogEntry CreateLogEntry(TraceEventType traceEventType)
        {
            LogEntry log = new LogEntry();
            log.Categories.Add(category);
            log.Priority = priority;
            log.Severity = traceEventType;
            return log;
        }

        /// <summary>
        /// Configures the log entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        protected virtual void ConfigureLogEntry(LogEntry log, object message)
        {
            ConfigureLogEntry(log, message, null);
        }

        /// <summary>
        /// Configures the log entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        protected virtual void ConfigureLogEntry(LogEntry log, object message, Exception ex)
        {
            string nullSafeMessage = (message == null ? null : message.ToString());
            if (ex == null)
            {
                log.Message = nullSafeMessage;
            }
            else
            {
                log.Message = AddExceptionInfo(nullSafeMessage, ex);
            }            
        }

        /// <summary>
        /// Adds the exception info.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        protected virtual string AddExceptionInfo(string message, Exception exception)
        {
            if (exception != null)
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append(message).Append(separator);
                sb.Append("Exception[ ");
                sb.Append("message = ").Append(exception.Message).Append(separator);
                sb.Append("source = ").Append(exception.Source).Append(separator);
                sb.Append("targetsite = ").Append(exception.TargetSite).Append(separator);
                sb.Append("stacktrace = ").Append(exception.StackTrace).Append("]");
                return sb.ToString();
            }
            return message;
        }
    }
}