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
using System.IO;
using System.Reflection;
using Common.Logging.Simple;
using log4net.Config;
using log4net.Core;
using NUnit.Framework;

namespace Common.Logging.Log4Net
{
    /// <summary>
    /// </summary>
    /// <author>Erich Eichinger</author>
    [TestFixture]
    public class CommonLoggingAppenderTests
    {
        [Test]
        public void RoutesToCommonLogging()
        {
            //            CommonLoggingAppender appender = new CommonLoggingAppender();
            //            appender.Layout = new PatternLayout("%level - %class.%method: %message");
            //            BasicConfigurator.Configure(stm);

            Stream stm = this.GetType().Assembly.GetManifestResourceStream(this.GetType().FullName + "_log4net.config.xml");
            XmlConfigurator.Configure(stm);

            CapturingLoggerFactoryAdapter adapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = adapter;
            
            string message = "testmessage";
            Exception exception = new Exception("testexception");

            adapter.ClearLastEvent();
            log4net.LogManager.GetLogger(this.GetType()).Debug(message, exception);
            Assert.AreEqual(this.GetType().FullName, adapter.LastEvent.Source.Name);
            Assert.AreEqual(string.Format("{0} - {1}.{2}: {3}", Level.Debug, this.GetType().FullName, MethodBase.GetCurrentMethod().Name ,  message), adapter.LastEvent.MessageObject.ToString());
            Assert.AreSame(exception, adapter.LastEvent.Exception);

            adapter.ClearLastEvent();
            log4net.LogManager.GetLogger(this.GetType()).Warn(message, exception);
            Assert.AreEqual(this.GetType().FullName, adapter.LastEvent.Source.Name);
            Assert.AreEqual(string.Format("{0} - {1}.{2}: {3}", Level.Warn, this.GetType().FullName, MethodBase.GetCurrentMethod().Name ,  message), adapter.LastEvent.MessageObject.ToString());
            Assert.AreSame(exception, adapter.LastEvent.Exception);
        }
    }
}