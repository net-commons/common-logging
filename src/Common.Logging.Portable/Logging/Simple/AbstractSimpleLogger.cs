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
using System.Globalization;
using System.Text;
using Common.Logging.Factory;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Abstract class providing a standard implementation of simple loggers.
    /// </summary>
    /// <author>Erich Eichinger</author>
#if !PORTABLE
    [Serializable]
#endif

    public abstract class AbstractSimpleLogger : AbstractLogger
    {
        private readonly string _name;
        private readonly bool _showLevel;
        private readonly bool _showDateTime;
        private readonly bool _showLogName;
        private LogLevel _currentLogLevel;
        private readonly string _dateTimeFormat;
        private readonly bool _hasDateTimeFormat;

        #region Properties

        /// <summary>
        /// The key of the logger.
        /// </summary>
        [CoverageExclude]
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Include the current log level in the log message.
        /// </summary>
        [CoverageExclude]
        public bool ShowLevel
        {
            get { return _showLevel; }
        }

        /// <summary>
        /// Include the current time in the log message.
        /// </summary>
        [CoverageExclude]
        public bool ShowDateTime
        {
            get { return _showDateTime; }
        }

        /// <summary>
        /// Include the instance key in the log message.
        /// </summary>
        [CoverageExclude]
        public bool ShowLogName
        {
            get { return _showLogName; }
        }

        /// <summary>
        /// The current logging threshold. Messages recieved that are beneath this threshold will not be logged.
        /// </summary>
        [CoverageExclude]
        public LogLevel CurrentLogLevel
        {
            get { return _currentLogLevel; }
            set { _currentLogLevel = value; }
        }

        /// <summary>
        /// The date and time format to use in the log message.
        /// </summary>
        [CoverageExclude]
        public string DateTimeFormat
        {
            get { return _dateTimeFormat; }
        }

        /// <summary>
        /// Determines Whether <see cref="DateTimeFormat"/> is set.
        /// </summary>
        [CoverageExclude]
        public bool HasDateTimeFormat
        {
            get { return _hasDateTimeFormat; }
        }


        #endregion

        /// <summary>
        /// Creates and initializes a the simple logger.
        /// </summary>
        /// <param name="logName">The key, usually type key of the calling class, of the logger.</param>
        /// <param name="logLevel">The current logging threshold. Messages recieved that are beneath this threshold will not be logged.</param>
        /// <param name="showlevel">Include level in the log message.</param>
        /// <param name="showDateTime">Include the current time in the log message.</param>
        /// <param name="showLogName">Include the instance key in the log message.</param>
        /// <param name="dateTimeFormat">The date and time format to use in the log message.</param>
        public AbstractSimpleLogger(string logName, LogLevel logLevel, bool showlevel
                                 , bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            _name = logName;
            _currentLogLevel = logLevel;
            _showLevel = showlevel;
            _showDateTime = showDateTime;
            _showLogName = showLogName;
            _dateTimeFormat = dateTimeFormat;
            _hasDateTimeFormat = (!string.IsNullOrEmpty(_dateTimeFormat));
        }

        /// <summary>
        /// Appends the formatted message to the specified <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">the <see cref="StringBuilder"/> that receíves the formatted message.</param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        protected virtual void FormatOutput(StringBuilder stringBuilder, LogLevel level, object message, Exception e)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException("stringBuilder");
            }

            // Append date-time if so configured
            if (_showDateTime)
            {
                if (_hasDateTimeFormat)
                {
                    stringBuilder.Append(DateTime.Now.ToString(_dateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    stringBuilder.Append(DateTime.Now);
                }

                stringBuilder.Append(" ");
            }

            if (_showLevel)
            {
                // Append a readable representation of the log level
                stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));
            }

            // Append the key of the log instance if so configured
            if (_showLogName)
            {
                stringBuilder.Append(_name).Append(" - ");
            }

            // Append the message
            stringBuilder.Append(message);

            // Append stack trace if not null
            if (e != null)
            {
                stringBuilder.Append(Environment.NewLine).Append(ExceptionFormatter.Format(e));
            }
        }

        /// <summary>
        /// Determines if the given log level is currently enabled.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual bool IsLevelEnabled(LogLevel level)
        {
            int iLevel = (int)level;
            int iCurrentLogLevel = (int)_currentLogLevel;

            // return iLevel.CompareTo(iCurrentLogLevel); better ???
            return (iLevel >= iCurrentLogLevel);
        }

        #region ILog Members

        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
        /// equal to <see cref="LogLevel.Trace" />. If it is, all messages will be sent to <see cref="Console.Out" />.
        /// </summary>
        public override bool IsTraceEnabled
        {
            get { return IsLevelEnabled(LogLevel.Trace); }
        }

        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
        /// equal to <see cref="LogLevel.Debug" />. If it is, all messages will be sent to <see cref="Console.Out" />.
        /// </summary>
        public override bool IsDebugEnabled
        {
            get { return IsLevelEnabled(LogLevel.Debug); }
        }

        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
        /// equal to <see cref="LogLevel.Info" />. If it is, only messages with a <see cref="LogLevel" /> of
        /// <see cref="LogLevel.Info" />, <see cref="LogLevel.Warn" />, <see cref="LogLevel.Error" />, and 
        /// <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
        /// </summary>
        public override bool IsInfoEnabled
        {
            get { return IsLevelEnabled(LogLevel.Info); }
        }


        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
        /// equal to <see cref="LogLevel.Warn" />. If it is, only messages with a <see cref="LogLevel" /> of
        /// <see cref="LogLevel.Warn" />, <see cref="LogLevel.Error" />, and <see cref="LogLevel.Fatal" /> 
        /// will be sent to <see cref="Console.Out" />.
        /// </summary>
        public override bool IsWarnEnabled
        {
            get { return IsLevelEnabled(LogLevel.Warn); }
        }

        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
        /// equal to <see cref="LogLevel.Error" />. If it is, only messages with a <see cref="LogLevel" /> of
        /// <see cref="LogLevel.Error" /> and <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
        /// </summary>
        public override bool IsErrorEnabled
        {
            get { return IsLevelEnabled(LogLevel.Error); }
        }

        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
        /// equal to <see cref="LogLevel.Fatal" />. If it is, only messages with a <see cref="LogLevel" /> of
        /// <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
        /// </summary>
        public override bool IsFatalEnabled
        {
            get { return IsLevelEnabled(LogLevel.Fatal); }
        }

        #endregion
    }
}
