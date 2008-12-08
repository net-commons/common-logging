using System;
using System.Collections;
using System.Collections.Specialized;
using Common.Logging;
using NUnit.Framework;

namespace Common.TestUtil
{
    public class TestLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        private readonly Hashtable loggers = CollectionsUtil.CreateCaseInsensitiveHashtable();

        public volatile TestLoggerEvent LastEvent;
        public readonly IList LoggerEvents = new ArrayList();

        public void AddEvent(TestLoggerEvent loggerEvent)
        {
            LastEvent = loggerEvent;
            lock(LoggerEvents.SyncRoot)
            {
                LoggerEvents.Add(loggerEvent);
            }    
        }

        public ILog GetLogger(Type type)
        {
            Assert.Fail("trace listener should obtain a logger by name");
            return null;
        }

        public ILog GetLogger(string name)
        {
            ILog logger = (ILog) loggers[name];
            if (logger == null)
            {
                lock(loggers.SyncRoot)
                {
                    logger = (ILog)loggers[name];
                    if (logger == null)
                    {
                        logger = new TestLogger(this, name);
                        loggers[name] = logger;
                    }
                }
            }
            return logger;
        }
    }
}