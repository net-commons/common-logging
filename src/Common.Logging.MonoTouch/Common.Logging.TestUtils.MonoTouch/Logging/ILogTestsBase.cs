#region License

/*
 * Copyright © 2002-2006 the original author or authors.
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
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using NUnit.Framework;
using Common.TestUtil;

namespace Common.Logging
{
    /// <summary>
    /// Generic tests that can be applied to any log implementation by
    /// subclassing and defining the property LogObject.
    /// </summary>
    /// <remarks>
    /// Exercises basic API of the ILog implemetation.
    /// </remarks>
    /// <author>Mark Pollack</author>
    //    [TestFixture]
    public abstract class ILogTestsBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            //Security = new SecurityTemplate(true);
            LogManager.Reset();
            LogManager.Adapter = GetLoggerFactoryAdapter();
        }

        protected abstract ILoggerFactoryAdapter GetLoggerFactoryAdapter();

        [Test]
        public void LoggingWithNullParameters()
        {
            Console.Out.WriteLine("Executing Test " + MethodBase.GetCurrentMethod().Name);
            ILog log = LogManager.GetCurrentClassLogger();

            log.Trace((object)null);
            log.Trace((object)null, null);
            log.Trace((Action<FormatMessageHandler>)null);
            log.Trace((Action<FormatMessageHandler>)null, null);
            log.Trace(null, (Action<FormatMessageHandler>)null);
            log.Trace(null, (Action<FormatMessageHandler>)null, null);
            log.TraceFormat((string)null);
            log.TraceFormat(null, (Exception)null);
            log.TraceFormat((IFormatProvider)null, null);
            log.TraceFormat((IFormatProvider)null, null, (Exception)null);

            log.Debug((object)null);
            log.Debug((object)null, null);
            log.Debug((Action<FormatMessageHandler>)null);
            log.Debug((Action<FormatMessageHandler>)null, null);
            log.Debug(null, (Action<FormatMessageHandler>)null);
            log.Debug(null, (Action<FormatMessageHandler>)null, null);
            log.Debug(null);
            log.Debug(null, (Exception)null);
            log.Debug((IFormatProvider)null, null);
            log.Debug((IFormatProvider)null, null, (Exception)null);

        }

        [Test]
        public void CanCallIsEnabledFromNamedLog()
        {
            CanCallIsEnabled(LogManager.GetLogger("loggerName"));
        }

        [Test]
        public void CanLogMessageFromNamedLog()
        {
            CanLogMessage(LogManager.GetLogger("logger2Name"));
        }

        [Test]
        public void CanLogMessageWithExceptionFromNamedLog()
        {
            ILog log = LogManager.GetLogger("logger3Name");
            CanLogMessageWithException(log);
        }

        [Test]
        public void CanLogMessageWithExceptionFromTypeLog()
        {
            ILog log = LogManager.GetCurrentClassLogger();
            CanLogMessageWithException(log);
        }

        protected virtual void CanCallIsEnabled(ILog log)
        {
            bool b;
            b = log.IsTraceEnabled;
            b = log.IsDebugEnabled;
            b = log.IsInfoEnabled;
            b = log.IsWarnEnabled;
            b = log.IsErrorEnabled;
            b = log.IsFatalEnabled;
        }

        protected virtual void CanLogMessage(ILog log)
        {
            log.Trace("Hi Trace");
            log.Debug("Hi Debug");
            log.Info("Hi Info");
            log.Warn("Hi Warn");
            log.Error("Hi Error");
            log.Fatal("Hi Fatal");
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
            log.TraceFormat("Hi {0}", new ArithmeticException(), "Trace");
            log.DebugFormat("Hi {0}", new ArithmeticException(), "Debug");
            log.InfoFormat("Hi {0}", new ArithmeticException(), "Info");
            log.WarnFormat("Hi {0}", new ArithmeticException(), "Warn");
            log.ErrorFormat("Hi {0}", new ArithmeticException(), "Error");
            log.FatalFormat("Hi {0}", new ArithmeticException(), "Fatal");
        }
#endif

        protected delegate TResult Func<TResult, TArg1>(TArg1 arg1);

        protected MethodBase GetMember<TResult, TArg1>(Func<TResult, TArg1> action)
        {
            return action.Method;
        }
    }
}