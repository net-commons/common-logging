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
using System.Diagnostics;
using Common.Logging.Configuration;
using Common.Logging.Simple;
using NUnit.Framework;
//using Rhino.Mocks;

namespace Common.Logging
{
    /// <summary>
    /// Tests for LogManager that exercise the basic API and check for error conditions.
    /// </summary>
    /// <author>Mark Pollack</author>
    [TestFixture]
    public class LogManagerTests
    {
        //public MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            LogManager.Reset();
            //mocks = new MockRepository();
        }

        [Test]
        public void AdapterProperty()
        {
            ILoggerFactoryAdapter adapter = new NoOpLoggerFactoryAdapter();
            LogManager.Adapter = adapter;
            Assert.AreSame(adapter, LogManager.Adapter);

            Assert.Throws<ArgumentNullException>(delegate { LogManager.Adapter = null; });
        }

		[Test]
		public void ConfigureFromSettings ()
		{
			var logger = LogManager.GetCurrentClassLogger();
			logger.Info("Hi from plist config!");
		}
    }
}