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
using System.Runtime.Serialization;
using NUnit.Framework;

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
    /// <version>$Id:$</version>
    [TestFixture]
    public abstract class AbstractLogTest
    {
        public abstract ILog LogObject { get; }

        [Test]
        public void LoggingWithNullParameters()
        {
            ILog log = LogObject;

            Assert.IsNotNull(log);


            log.Debug(null);

            log.Debug(null, null);

            log.Debug(log.GetType().FullName + ": debug statement");

            log.Debug(log.GetType().FullName + ": debug statement w/ exception", new Exception("exception message"));


            log.Error(null);

            log.Error(null, null);

            log.Error(log.GetType().FullName + ": error statement");

            log.Error(log.GetType().FullName + ": error statement w/ exception", new Exception("exception message"));


            log.Fatal(null);

            log.Fatal(null, null);

            log.Fatal(log.GetType().FullName + ": fatal statement");

            log.Fatal(log.GetType().FullName + ": fatal statement w/ exception", new Exception("exception message"));


            log.Info(null);

            log.Info(null, null);

            log.Info(log.GetType().FullName + ": info statement");

            log.Info(log.GetType().FullName + ": info statement w/ exception", new Exception("exception message"));


            log.Warn(null);

            log.Warn(null, null);

            log.Warn(log.GetType().FullName + ": warn statement");

            log.Warn(log.GetType().FullName + ": warn statement w/ exception", new Exception("exception message"));
        }

        public void LoggerIsSerializable(ILog logger)
        {
            //TODO assign some non-default state to the logger...?

            if (logger.GetType().IsSerializable)
            {
                SerializationTestUtils.TrySerialization(logger);
                Assert.IsTrue(SerializationTestUtils.IsSerializable(logger));
                ILog logger2 = (ILog) SerializationTestUtils.SerializeAndDeserialize(logger);
                Assert.IsTrue(logger != logger2);
            }
        }
    }
}