#region License

/*
 * Copyright 2002-2009 the original author or authors.
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
using System.Collections;
using System.Globalization;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using FormatMessageCallback = System.Action<Common.Logging.FormatMessageHandler>;
using Is=Rhino.Mocks.Constraints.Is;

namespace Common.Logging.Factory
{
    /// <summary>
    /// Tests for <see cref="AbstractLogger"/> class. In particular ensures, that all ILog.XXX methods
    /// correctuly route to the single WriteInternal delegate.
    /// </summary>
    [TestFixture]
    public class AbstractLoggerTests
    {
        [Test]
        public void IsSerializable()
        {
            SerializationTestUtils.IsSerializable<AbstractLogger>();
        }

        [Test]
        public void ImplementsAllMethodsForAllLevels()
        {
            string[] logLevels = Exclude(Enum.GetNames(typeof (LogLevel)), "All", "Off");

            foreach (string logLevel in logLevels)
            {
                MethodInfo[] logMethods = GetLogMethodSignatures(logLevel);
                for (int i = 0; i < logLevels.Length; i++)
                {
                    Assert.IsNotNull(logMethods[i],
                                     "Method with signature #{0} not implemented for level {1}", i, logLevel);
                }
            }
        }

        [Test]
        public void LogsMessage()
        {
            string[] logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");

            foreach (string logLevel in logLevels)
            {
                LogsMessage(logLevel);
            }
        }

        /// <summary>
        /// Ensures, that all interface methods delegate to Write() with correct level + arguments
        /// and that arguments are still not evaluated up to this point (e.g. calling ToString())
        /// </summary>
        private static void LogsMessage(string levelName)
        {
            MockRepository mocks = new MockRepository();

            TestLogger log = new TestLogger();
            Exception ex = new Exception();


            MethodInfo[] logMethods = GetLogMethodSignatures(levelName);

            LogLevel logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), levelName);

            Invoke(log, logMethods[0], "messageObject0");
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("messageObject0", log.LastMessage);
            Assert.AreEqual(null, log.LastException);

            Invoke(log, logMethods[1], "messageObject1", ex);
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("messageObject1", log.LastMessage);
            Assert.AreEqual(ex, log.LastException);

            Invoke(log, logMethods[2], "format2 {0}", new object[] { "arg2" });
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("format2 arg2", log.LastMessage);
            Assert.AreEqual(null, log.LastException);

            Invoke(log, logMethods[3], "format3 {0}", ex, new object[] { "arg3" });
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("format3 arg3", log.LastMessage);
            Assert.AreEqual(ex, log.LastException);

            Invoke(log, logMethods[4], CultureInfo.CreateSpecificCulture("de-de"), "format4 {0}", new object[] { 4.1 });
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("format4 4,1", log.LastMessage);
            Assert.AreEqual(null, log.LastException);

            Invoke(log, logMethods[5], CultureInfo.CreateSpecificCulture("de-de"), "format5 {0}", ex, new object[] { 5.1 });
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("format5 5,1", log.LastMessage);
            Assert.AreEqual(ex, log.LastException);

            Invoke(log, logMethods[6], TestFormatMessageCallback.MessageCallback("message6"));
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("message6", log.LastMessage);
            Assert.AreEqual(null, log.LastException);

            Invoke(log, logMethods[7], TestFormatMessageCallback.MessageCallback("message7"), ex);
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("message7", log.LastMessage);
            Assert.AreEqual(ex, log.LastException);

            Invoke(log, logMethods[8], CultureInfo.CreateSpecificCulture("de-de"), TestFormatMessageCallback.MessageCallback("format8 {0}", new object[] { 8.1 }));
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("format8 8,1", log.LastMessage);
            Assert.AreEqual(null, log.LastException);

            Invoke(log, logMethods[9], CultureInfo.CreateSpecificCulture("de-de"), TestFormatMessageCallback.MessageCallback("format9 {0}", new object[] { 9.1 }), ex);
            Assert.AreEqual(logLevel, log.LastLogLevel);
            Assert.AreEqual("format9 9,1", log.LastMessage);
            Assert.AreEqual(ex, log.LastException);
        }

        [Test]
        public void WriteIsCalledWithCorrectLogLevel()
        {
            string[] logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");

            foreach (string logLevel in logLevels)
            {
                WriteIsCalledWithCorrectLogLevel(logLevel);
            }            
        }

        /// <summary>
        /// Ensures, that all interface methods delegate to Write() with correct level + arguments
        /// and that arguments are still not evaluated up to this point (e.g. calling ToString())
        /// </summary>
        private static void WriteIsCalledWithCorrectLogLevel(string levelName)
        {
            MockRepository mocks = new MockRepository();

            AbstractTestLogger log = (AbstractTestLogger)mocks.PartialMock(typeof(AbstractTestLogger));
            Exception ex = (Exception)mocks.StrictMock(typeof(Exception));
            object messageObject = mocks.StrictMock(typeof(object));
            object formatArg = mocks.StrictMock(typeof(object));
            FormatMessageCallback failCallback = TestFormatMessageCallback.FailCallback();

            MethodInfo[] logMethods = GetLogMethodSignatures(levelName);

            LogLevel logLevel = (LogLevel) Enum.Parse(typeof (LogLevel), levelName);

            using (mocks.Ordered())
            {
                log.Log(logLevel, null, null);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
                log.Log(logLevel, null, ex);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
                log.Log(logLevel, null, null);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
                log.Log(logLevel, null, ex);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
                log.Log(logLevel, null, null);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
                log.Log(logLevel, null, ex);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
                log.Log(logLevel, null, null);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
                log.Log(logLevel, null, ex);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
                log.Log(logLevel, null, null);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
                log.Log(logLevel, null, ex);
                LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
            }
            mocks.ReplayAll();

            Invoke(log, logMethods[0], messageObject);
            Invoke(log, logMethods[1], messageObject, ex);
            Invoke(log, logMethods[2], "format", new object[] { formatArg });
            Invoke(log, logMethods[3], "format", ex, new object[] { formatArg });
            Invoke(log, logMethods[4], CultureInfo.InvariantCulture, "format", new object[] { formatArg });
            Invoke(log, logMethods[5], CultureInfo.InvariantCulture, "format", ex, new object[] { formatArg });
            Invoke(log, logMethods[6], failCallback);
            Invoke(log, logMethods[7], failCallback, ex);
            Invoke(log, logMethods[8], CultureInfo.InvariantCulture, failCallback);
            Invoke(log, logMethods[9], CultureInfo.InvariantCulture, failCallback, ex);

            mocks.VerifyAll();
        }

        [Test]
        public void WriteAndEvaluateOnlyWhenLevelEnabled()
        {
            string[] logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");

            foreach (string logLevel in logLevels)
            {
                WriteAndEvaluateOnlyWhenLevelEnabled(logLevel);                
            }
        }

        /// <summary>
        /// This test ensures, that for a given loglevel
        /// a) <c>AbstractLogger.Write</c> is not called if that loglevel is disabled
        /// b) No argument is evaluated (e.g. calling ToString()) if that loglevel is disabled
        /// </summary>
        private static void WriteAndEvaluateOnlyWhenLevelEnabled(string levelName)
        {
            MockRepository mocks = new MockRepository();

            AbstractLogger log = (AbstractLogger)mocks.StrictMock(typeof(AbstractLogger));
            Exception ex = (Exception)mocks.StrictMock(typeof(Exception));
            object messageObject = mocks.StrictMock(typeof(object));
            object formatArg = mocks.StrictMock(typeof(object));
            FormatMessageCallback failCallback = TestFormatMessageCallback.FailCallback();

            MethodInfo[] logMethods = GetLogMethodSignatures(levelName);

            using (mocks.Ordered())
            {
                Invoke(log, logMethods[0], messageObject);
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[1], messageObject, ex);
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[2], "format", new object[] { formatArg });
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[3], "format", ex, new object[] { formatArg });
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[4], CultureInfo.InvariantCulture, "format", new object[] { formatArg });
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[5], CultureInfo.InvariantCulture, "format", ex, new object[] { formatArg });
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[6], failCallback);
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[7], failCallback, ex);
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[8], CultureInfo.InvariantCulture, failCallback);
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
                Invoke(log, logMethods[9], CultureInfo.InvariantCulture, failCallback, ex);
                LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
                Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            }
            mocks.ReplayAll();

            Invoke(log, logMethods[0], messageObject);
            Invoke(log, logMethods[1], messageObject, ex);
            Invoke(log, logMethods[2], "format", new object[] {formatArg});
            Invoke(log, logMethods[3], "format", ex, new object[] { formatArg });
            Invoke(log, logMethods[4], CultureInfo.InvariantCulture, "format", new object[] {formatArg});
            Invoke(log, logMethods[5], CultureInfo.InvariantCulture, "format", ex, new object[] { formatArg });
            Invoke(log, logMethods[6], failCallback);
            Invoke(log, logMethods[7], failCallback, ex);
            Invoke(log, logMethods[8], CultureInfo.InvariantCulture, failCallback);
            Invoke(log, logMethods[9], CultureInfo.InvariantCulture, failCallback, ex);

            mocks.VerifyAll();
        }

        private static bool IsLevelEnabled(ILog log, string logLevelName)
        {
            switch(logLevelName)
            {
                case "Trace":
                    return log.IsTraceEnabled;
                case "Debug":
                    return log.IsDebugEnabled;
                case "Info":
                    return log.IsInfoEnabled;
                case "Warn":
                    return log.IsWarnEnabled;
                case "Error":
                    return log.IsErrorEnabled;
                case "Fatal":
                    return log.IsFatalEnabled;
                default:
                    throw new ArgumentOutOfRangeException("logLevelName", logLevelName, "unknown log level ");
            }
        }

        private static readonly Type[][] methodSignatures =
            {
                new Type[] {typeof (object)},
                new Type[] {typeof (object), typeof (Exception)},
                new Type[] {typeof (string), typeof (object[])},
                new Type[] {typeof (string), typeof (Exception), typeof (object[])},
                new Type[] {typeof (IFormatProvider), typeof (string), typeof (object[])},
                new Type[] {typeof (IFormatProvider), typeof (string), typeof (Exception), typeof (object[])},
                new Type[] {typeof (FormatMessageCallback)},
                new Type[] {typeof (FormatMessageCallback), typeof (Exception)},
                new Type[] {typeof (IFormatProvider), typeof (FormatMessageCallback)},
                new Type[] {typeof (IFormatProvider), typeof (FormatMessageCallback), typeof (Exception)}
            };

        private static MethodInfo[] GetLogMethodSignatures(string levelName)
        {
            return new MethodInfo[]
                       {
                           typeof (ILog).GetMethod(levelName, methodSignatures[0]),
                           typeof (ILog).GetMethod(levelName, methodSignatures[1]),
                           typeof (ILog).GetMethod(levelName + "Format", methodSignatures[2]),
                           typeof (ILog).GetMethod(levelName + "Format", methodSignatures[3]),
                           typeof (ILog).GetMethod(levelName + "Format", methodSignatures[4]),
                           typeof (ILog).GetMethod(levelName + "Format", methodSignatures[5]),
                           typeof (ILog).GetMethod(levelName, methodSignatures[6]),
                           typeof (ILog).GetMethod(levelName, methodSignatures[7]),
                           typeof (ILog).GetMethod(levelName, methodSignatures[8]),
                           typeof (ILog).GetMethod(levelName, methodSignatures[9])
                       };
        }

        private static void Invoke(object target, MethodInfo method, params object[] args)
        {
            method.Invoke(target, args);
        }

        #region TestFormatMessageCallback Utility Class

        private class TestFormatMessageCallback
        {
            private readonly bool throwOnInvocation;
            private readonly string messageToReturn;
            private readonly object[] argsToReturn;

            private TestFormatMessageCallback(bool throwOnInvocation)
            {
                this.throwOnInvocation = throwOnInvocation;
            }

            private TestFormatMessageCallback(string messageToReturn, object[] args)
            {
                this.messageToReturn = messageToReturn;
                this.argsToReturn = args;
            }

            public static FormatMessageCallback FailCallback()
            {
                return new FormatMessageCallback(new TestFormatMessageCallback(true).FormatMessage);
            }

            public static FormatMessageCallback MessageCallback(string message, params object[] args)
            {
                return new FormatMessageCallback(new TestFormatMessageCallback(message, args).FormatMessage);
            }

            private void FormatMessage(FormatMessageHandler fmh)
            {
                if (throwOnInvocation)
                {
                    Assert.Fail();
                }
                fmh(messageToReturn, argsToReturn);
            }
        }

        #endregion

        private static string[] Exclude(string[] arr, params string[] exclude)
        {
            ArrayList result = new ArrayList();
            foreach (string s in arr)
            {
                if (0 > Array.BinarySearch(exclude, s))
                {
                    result.Add(s);
                }
            }
            return (string[]) result.ToArray(typeof (string));
        }

        public class TestLogger : AbstractTestLogger
        {
            public LogLevel LastLogLevel;
            public string LastMessage;
            public Exception LastException;

            protected override WriteHandler GetWriteHandler()
            {
                return new WriteHandler(Log);
            }

            protected override void WriteInternal(LogLevel level, object message, Exception exception)
            {
                Assert.Fail("must never been called - Log() should be called");
            }

            public override void Log(LogLevel level, object message, Exception exception)
            {
                LastLogLevel = level;
                LastMessage = message.ToString();
                LastException = exception;
            }
        }

        public abstract class AbstractTestLogger : AbstractLogger
        {
            protected override void WriteInternal(LogLevel level, object message, Exception exception)
            {
                Log(level, message, exception);
            }

            public abstract void Log(LogLevel level, object message, Exception exception);

            /// <summary>
            /// Checks if this logger is enabled for the <see cref="LogLevel.Trace"/> level.
            /// </summary>
            public override bool IsTraceEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// Checks if this logger is enabled for the <see cref="LogLevel.Debug"/> level.
            /// </summary>
            public override bool IsDebugEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// Checks if this logger is enabled for the <see cref="LogLevel.Error"/> level.
            /// </summary>
            public override bool IsErrorEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// Checks if this logger is enabled for the <see cref="LogLevel.Fatal"/> level.
            /// </summary>
            public override bool IsFatalEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// Checks if this logger is enabled for the <see cref="LogLevel.Info"/> level.
            /// </summary>
            public override bool IsInfoEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// Checks if this logger is enabled for the <see cref="LogLevel.Warn"/> level.
            /// </summary>
            public override bool IsWarnEnabled
            {
                get { return true; }
            }
        }
    }
}