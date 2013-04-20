using System;

namespace Common.Logging.Simple
{
    /// <summary>
    /// A logging event captured by <see cref="CapturingLogger"/>
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class CapturingLoggerEvent
    {
        /// <summary>
        /// The logger that logged this event
        /// </summary>
        public readonly CapturingLogger Source;
        /// <summary>
        /// The level used to log this event
        /// </summary>
        public readonly LogLevel Level;
        /// <summary>
        /// The raw message object
        /// </summary>
        public readonly object MessageObject;
        /// <summary>
        /// A logged exception
        /// </summary>
        public readonly Exception Exception;

        /// <summary>
        /// Retrieves the formatted message text
        /// </summary>
        public string RenderedMessage
        {
            get { return MessageObject.ToString(); }
        }

        /// <summary>
        /// Create a new event instance
        /// </summary>
        public CapturingLoggerEvent(CapturingLogger source, LogLevel level, object messageObject, Exception exception)
        {
            Source = source;
            Level = level;
            MessageObject = messageObject;
            Exception = exception;
        }
    }
}