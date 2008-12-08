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
    /// <version>$Id:$</version>    
    public abstract class AbstractSimpleLogTest : AbstractLogTest
    {
        protected ILog defaultLogInstance;
        protected static int count;

        protected static NameValueCollection GetProperties()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            if ((count%2) == 0)
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

        protected virtual void CanLogMessageWithException(ILog log)
        {
            log.TraceFormat("Hi");
            log.Debug("Hi", new ArithmeticException());
            log.Info("Hi", new ArithmeticException());
            log.Warn("Hi", new ArithmeticException());
            log.Error("Hi", new ArithmeticException());
            log.Fatal("Hi", new ArithmeticException());
        }

        /// <summary>
        /// Basic sanity checks of default values for a log implementation.
        /// </summary>
        /// <param name="log">A log implementation</param>
        protected abstract void CheckLog(ILog log);
    }
}