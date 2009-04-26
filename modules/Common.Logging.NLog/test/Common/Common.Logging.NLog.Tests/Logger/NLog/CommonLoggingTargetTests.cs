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
using Common.Logging;
using Common.Logging.NLog;
using Common.Logging.Simple;
using NLog.Config;
using NUnit.Framework;
using LogLevel=NLog.LogLevel;
using LogManager = Common.Logging.LogManager;
using NLogLogManager = NLog.LogManager;

namespace Common.Logger.NLog
{
    /// <summary>
    /// </summary>
    /// <author>Erich Eichinger</author>
    [TestFixture]
    public class CommonLoggingTargetTests
    {
        [Test]
        public void RoutesToCommonLogging()
        {
            LoggingConfiguration cfg = new LoggingConfiguration();
            CommonLoggingTarget target = new CommonLoggingTarget("${level:uppercase=true}|${logger}|${message}");
            cfg.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));

            NLogLogManager.Configuration = cfg;

            // configure for capturing
            CapturingLoggerFactoryAdapter adapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = adapter;

            string msg = "testmessage";

            Exception ex = new Exception("testexception");

            adapter.ClearLastEvent();
            NLogLogManager.GetLogger("myLogger").DebugException(msg, ex);
            Assert.AreEqual("myLogger", adapter.LastEvent.Source.Name);
            Assert.AreEqual(string.Format("DEBUG|myLogger|{0}", msg), adapter.LastEvent.RenderedMessage);
            Assert.AreSame(ex, adapter.LastEvent.Exception);
        }
    }
}