using System;
using Common.Logging.Configuration;

namespace Common.Logging.Simple
{
    /// <summary>
    /// A logger created by <see cref="CapturingLoggerFactoryAdapter"/> that 
    /// sends all log events to the owning adapter's <see cref="CapturingLoggerFactoryAdapter.AddEvent"/>
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class CapturingLogger : AbstractSimpleLogger
    {
        /// <summary>
        /// The adapter that created this logger instance.
        /// </summary>
        public readonly CapturingLoggerFactoryAdapter Owner;

        ///<summary>
        /// Allows to retrieve the last logged event instance captured by this logger
        ///</summary>
        public CapturingLoggerEvent LastEvent;

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
            LastEvent = new CapturingLoggerEvent(this, level, message, exception);
            Owner.AddEvent(LastEvent);
        }
    }
}