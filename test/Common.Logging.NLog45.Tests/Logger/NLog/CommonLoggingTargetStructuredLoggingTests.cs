using Common.Logging.Simple;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FactoryNLog = NLog.LogFactory;
using LoggerNLog = NLog.Logger;
using ConfigNLog = NLog.Config.LoggingConfiguration;

namespace Common.Logging.NLog
{
    public class CommonLoggingTargetStructuredLoggingTests
    {
        [Test]
        public void RoutesToCommonLogging()
        {
            // configure for capturing
            CapturingLoggerFactoryAdapter adapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = adapter;
            FactoryNLog logFactory = new FactoryNLog();
            ConfigNLog logConfig = new ConfigNLog();
            logConfig.AddRuleForAllLevels(new CommonLoggingTarget() { Layout = "${message}" });
            logFactory.Configuration = logConfig;
            LoggerNLog logger = logFactory.GetCurrentClassLogger();

            Exception exception = new Exception();

            adapter.ClearLastEvent();

            object position = new { Latitude = 25, Longitude = 134 };
            int elapsedMs = 34;
            logger.Error(exception, "Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            Assert.AreEqual(typeof(CommonLoggingTargetStructuredLoggingTests).FullName, adapter.LastEvent.Source.Name);
            Assert.AreEqual("Processed {\"Latitude\":25, \"Longitude\":134} in 034 ms.", adapter.LastEvent.RenderedMessage);
            Assert.AreSame(exception, adapter.LastEvent.Exception);
        }
    }
}
