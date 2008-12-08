using System;
using System.Collections;
using System.Collections.Specialized;

namespace Common.Logging
{
    /// <summary>
    /// An implementation of <see cref="ILoggerFactoryAdapter"/> that caches loggers handed out by this factory.
    /// </summary>
    /// <remarks>
    /// Implementors just need to override <see cref="CreateLogger"/>.
    /// </remarks>
    /// <author>Erich Eichinger</author>
    public abstract class AbstractCachingLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        private readonly Hashtable _cachedLoggers;

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="caseSensitiveLoggerCache"></param>
        protected AbstractCachingLoggerFactoryAdapter(bool caseSensitiveLoggerCache)
        {
            _cachedLoggers = (caseSensitiveLoggerCache) 
                ? new Hashtable()
                : CollectionsUtil.CreateCaseInsensitiveHashtable();
        }

        /// <summary>
        /// Purges all loggers from cache
        /// </summary>
        protected void ClearLoggerCache()
        {
            lock (_cachedLoggers)
            {
                _cachedLoggers.Clear();                
            }
        }

        /// <summary>
        /// Create the specified named logger instance
        /// </summary>
        /// <remarks>
        /// Derived factories need to implement this method to create the
        /// actual logger instance.
        /// </remarks>
        protected abstract ILog CreateLogger(string name);

        #region ILoggerFactoryAdapter Members

        /// <summary>
        /// Get a ILog instance by <see cref="Type" />.
        /// </summary>
        /// <param name="type">Usually the <see cref="Type" /> of the current class.</param>
        /// <returns>
        /// An ILog instance either obtained from the internal cache or created by a call to <see cref="CreateLogger"/>.
        /// </returns>
        public ILog GetLogger(Type type)
        {
            return GetLoggerInternal(type.FullName);
        }

        /// <summary>
        /// Get a ILog instance by name.
        /// </summary>
        /// <param name="name">Usually a <see cref="Type" />'s Name or FullName property.</param>
        /// <returns>
        /// An ILog instance either obtained from the internal cache or created by a call to <see cref="CreateLogger"/>.
        /// </returns>
        public ILog GetLogger(string name)
        {
            return GetLoggerInternal(name);
        }

        /// <summary>
        /// Get or create a ILog instance by name.
        /// </summary>
        /// <param name="name">Usually a <see cref="Type" />'s Name or FullName property.</param>
        /// <returns>
        /// An ILog instance either obtained from the internal cache or created by a call to <see cref="CreateLogger"/>.
        /// </returns>
        private ILog GetLoggerInternal(string name)
        {
            ILog log = _cachedLoggers[name] as ILog;
            if (log == null)
            {
                lock (_cachedLoggers)
                {
                    log = _cachedLoggers[name] as ILog;
                    if (log == null)
                    {
                        log = CreateLogger(name);
                        if (log == null)
                        {
                            throw new ArgumentException(string.Format("{0} returned null on creating logger instance for name {1}", this.GetType().FullName, name));
                        }
                        _cachedLoggers.Add(name, log);
                    }
                }
            }
            return log;
        }

        #endregion
    }
}
