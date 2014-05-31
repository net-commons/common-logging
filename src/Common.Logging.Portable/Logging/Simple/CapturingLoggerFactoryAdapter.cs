using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Logging.Simple
{
    /// <summary>
    /// An adapter, who's loggers capture all log events and send them to <see cref="AddEvent"/>. 
    /// Retrieve the list of log events from <see cref="LoggerEvents"/>.
    /// </summary>
    /// <remarks>
    /// This logger factory is mainly for debugging and test purposes.
    /// <example>
    /// This is an example how you might use this adapter for testing:
    /// <code>
    /// // configure for capturing
    /// CapturingLoggerFactoryAdapter adapter = new CapturingLoggerFactoryAdapter();
    /// LogManager.Adapter = adapter;
    /// 
    /// // reset capture state
    /// adapter.Clear();
    /// // log something
    /// ILog log = LogManager.GetCurrentClassLogger();
    /// log.DebugFormat(&quot;Current Time:{0}&quot;, DateTime.Now);
    /// 
    /// // check logged data
    /// Assert.AreEqual(1, adapter.LoggerEvents.Count);
    /// Assert.AreEqual(LogLevel.Debug, adapter.LastEvent.Level);
    /// </code>
    /// </example>
    /// </remarks>
    /// <author>Erich Eichinger</author>
    public class CapturingLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        private readonly Dictionary<string, ILog> _cachedLoggers = new Dictionary<string, ILog>(StringComparer.OrdinalIgnoreCase);

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
            lock(LoggerEvents)
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
        }

        /// <summary>
        /// Get a <see cref="CapturingLogger"/> instance for the given type.
        /// </summary>
        public ILog GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        /// <summary>
        /// Get a <see cref="CapturingLogger"/> instance for the given key.
        /// </summary>
        public ILog GetLogger(string name)
        {
            ILog logger;
            if(!_cachedLoggers.TryGetValue(name, out logger))
            {
                lock (((ICollection)_cachedLoggers).SyncRoot)
                {
                    if (!_cachedLoggers.TryGetValue(name, out logger))
                    {
                        logger = new CapturingLogger(this, name);
                        _cachedLoggers[name] = logger;
                    }
                }
            }
            return logger;
        }
    }
}