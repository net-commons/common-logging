#region License

/*
 * Copyright © 2002-2007 the original author or authors.
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
using System.Collections.Specialized;
using System.Reflection;
using NUnit.Framework;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Base class that exercises the basic api of the simple loggers
    /// </summary>
    /// <author>Mark Pollack</author>
    public abstract class AbstractSimpleLogTest : AbstractLogTest
    {
        protected ILog defaultLogInstance;
        protected static int count;

        protected static NameValueCollection GetProperties()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            if ((count % 2) == 0)
            {
                properties["dateTimeFormat"] = "yyyy/MM/dd HH:mm:ss:fff";
            }
            count++;
            return properties;
        }

        public override ILog LogObject
        {
            get { return defaultLogInstance; }
        }

        public abstract Type LoggerType { get; }

        [Test]
        public void DefaultSettings()
        {
            CheckLog(LogObject);
        }

        [Test]
        public void LoggerSerialization()
        {
            LoggerIsSerializable(LogObject);
        }

        [Test]
        public void CanCallIsEnabledFromNamedLog()
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
            CanCallIsEnabled(log);
        }

        [Test]
        public void CanCallIsEnabledFromTypeLog()
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            CanCallIsEnabled(log);
        }

        [Test]
        public void CanLogMessageFromNamedLog()
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
            CanLogMessage(log);
        }

        [Test]
        public void CanLogMessageFromTypeLog()
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            CanLogMessage(log);
        }

        [Test]
        public void CanLogMessageWithExceptionFromNamedLog()
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
            CanLogMessageWithException(log);
        }

        [Test]
        public void CanLogMessageWithExceptionFromTypeLog()
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            CanLogMessageWithException(log);
        }

        protected virtual void CanCallIsEnabled(ILog log)
        {
            bool b;
            b = log.IsTraceEnabled;
            b = log.IsDebugEnabled;
            b = log.IsErrorEnabled;
            b = log.IsFatalEnabled;
            b = log.IsInfoEnabled;
            b = log.IsWarnEnabled;
        }

        protected virtual void CanLogMessage(ILog log)
        {
            log.TraceFormat("Hi");
            log.Debug("Hi");
            log.Info("Hi");
            log.Warn("Hi");
            log.Error("Hi");
            log.Fatal("Hi");
        }

#if NET_3_0
        protected virtual void CanLogMessageWithException(ILog log)
        {
            log.Trace(m => m("Hi {0}", "dude"));
            log.Debug(m => m("Hi {0}", "dude"), new ArithmeticException());
            log.Info(m => m("Hi {0}", "dude"), new ArithmeticException());
            log.Warn(m => m("Hi {0}", "dude"), new ArithmeticException());
            log.Error(m => m("Hi {0}", "dude"), new ArithmeticException());
            log.Fatal(m => m("Hi {0}", "dude"), new ArithmeticException());
        }
#else
        protected virtual void CanLogMessageWithException(ILog log)
        {
            log.TraceFormat("Hi {0}", new ArithmeticException(), "dude");
            log.DebugFormat("Hi {0}", new ArithmeticException(), "dude");
            log.InfoFormat("Hi {0}", new ArithmeticException(), "dude");
            log.WarnFormat("Hi {0}", new ArithmeticException(), "dude");
            log.ErrorFormat("Hi {0}", new ArithmeticException(), "dude");
            log.FatalFormat("Hi {0}", new ArithmeticException(), "dude");
        }
#endif

        /// <summary>
        /// Basic sanity checks of default values for a log implementation.
        /// </summary>
        /// <param name="log">A log implementation</param>
        protected abstract void CheckLog(ILog log);
    }
}
