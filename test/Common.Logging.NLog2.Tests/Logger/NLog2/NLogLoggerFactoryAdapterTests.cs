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

using System.Reflection;
using System.Security;
using Common.Logging;
using Common.Logging.NLog2;
using Common.TestUtil;
using NLog;
using NLog.Config;
using NUnit.Framework;
using LogLevel=NLog.LogLevel;
using LogManager=NLog.LogManager;

namespace Common.Logger.NLog2
{
    using NLog.Targets;

    [TestFixture]
    public class NLogLoggerFactoryAdapterTests : ILogTestsBase
    {
        private class TestLoggingConfiguration : LoggingConfiguration
        {
            public readonly TestTarget Target;

            public TestLoggingConfiguration()
            {
                Target = new TestTarget();
                LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, Target));
                
            }
        }

        private class TestTarget : TargetWithLayout
        {
            public LogEventInfo LastLogEvent;

            public TestTarget()
            {
                Layout = "${stacktrace}";
            }

            protected override void Write(LogEventInfo logEvent)
            {
                LastLogEvent = logEvent;
            }
        }

        [SetUp]
        public override void SetUp()
        {
            TestLoggingConfiguration cfg = new TestLoggingConfiguration();
            LogManager.Configuration = cfg;
            base.SetUp();
        }

        /// <summary>
        /// NLog lacks <see cref="AllowPartiallyTrustedCallersAttribute"/> 
        /// and therefore needs full trust enviroments.
        /// </summary>
        protected override string CompliantTrustLevelName
        {
            get
            {
                return SecurityTemplate.PERMISSIONSET_FULLTRUST;
            }
        }

        protected override ILoggerFactoryAdapter GetLoggerFactoryAdapter()
        {
            return new NLogLoggerFactoryAdapter(null);
        }

        [Test, Ignore("Fails due to a reason in NLog 2, see http://nlog.codeplex.com/workitem/6737 - this can be easily fixed.")]
        public void LogsUserStackFrame()
        {
            TestLoggingConfiguration cfg = new TestLoggingConfiguration();
            LogManager.Configuration = cfg;

            Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(null);
            Common.Logging.LogManager.GetLogger("myLogger").Debug("TestMessage");

            Assert.IsNotNull(cfg.Target.LastLogEvent);
            string stackTrace = cfg.Target.LastLogEvent.StackTrace.ToString();
            Assert.AreSame(MethodBase.GetCurrentMethod(), cfg.Target.LastLogEvent.UserStackFrame.GetMethod());
        }
    }
}
