using System;
using System.Collections.Generic;
using Common.Logging.Configuration;


namespace Common.Logging.Simple
{
    /// <summary>
    /// A logger created by <see cref="CapturingLoggerFactoryAdapter"/> that 
    /// sends all log events to the owning adapter's <see cref="CapturingLoggerFactoryAdapter.AddEvent"/>
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class CapturingLogger : Common.Logging.Simple.AbstractSimpleLogger
    {
        /// <summary>
        /// The adapter that created this logger instance.
        /// </summary>
        public readonly CapturingLoggerFactoryAdapter Owner;

        private volatile CapturingLoggerEvent _lastEvent;

        /// <summary>
        /// Holds the last log event received from any of this adapter's loggers.
        /// </summary>
        public CapturingLoggerEvent LastEvent
        {
            get { return _lastEvent; }
        }

        /// <summary>
        /// Clears all captured events
        /// </summary>
        public void Clear()
        {
            lock (LoggerEvents)
            {
                ClearLastEvent();
                LoggerEvents.Clear();
            }
        }

        /// <summary>
        /// Resets the <see cref="LastEvent"/> to <c>null</c>.
        /// </summary>
        public void ClearLastEvent()
        {
            _lastEvent = null;
        }

        /// <summary>
        /// Holds the list of logged events.
        /// </summary>
        /// <remarks>
        /// To access this collection in a multithreaded application, put a lock on the list instance.
        /// </remarks>
        public readonly IList<CapturingLoggerEvent> LoggerEvents = new List<CapturingLoggerEvent>();

        /// <summary>
        /// <see cref="CapturingLogger"/> instances send their captured log events to this method.
        /// </summary>
        public virtual void AddEvent(CapturingLoggerEvent loggerEvent)
        {
            _lastEvent = loggerEvent;
            lock (LoggerEvents)
            {
                LoggerEvents.Add(loggerEvent);
            }
            Owner.AddEvent(LastEvent);
        }

        /// <summary>
        /// Create a new logger instance.
        /// </summary>
        public CapturingLogger(CapturingLoggerFactoryAdapter owner, string logName)
            : base(logName, LogLevel.All, true, true, true, null)
        {
            ArgUtils.AssertNotNull("owner", owner);
            Owner = owner;
        }

        /// <summary>
        /// Create a new <see cref="CapturingLoggerEvent"/> and send it to <see cref="CapturingLoggerFactoryAdapter.AddEvent"/>
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            CapturingLoggerEvent ev = new CapturingLoggerEvent(this, level, message, exception);
            AddEvent(ev);
        }
    }
}