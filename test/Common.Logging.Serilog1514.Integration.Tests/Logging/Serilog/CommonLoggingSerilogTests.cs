using Common.Logging.Simple;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using serilogLogger = Serilog;

namespace Common.Logging.Serilog
{
    public class CommonLoggingSerilogTests
    {       
        [Test]
        public void RoutesToCommonLogging()
        {

            // configure for capturing
            CapturingLoggerFactoryAdapter adapter = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = adapter;
            var configuration = new LoggerConfiguration().ReadFrom.AppSettings().Enrich.WithProperty("Common.Logging.Type", typeof(CommonLoggingSerilogTests).FullName);
            var logger = configuration.CreateLogger();

            var exception = new Exception();

            adapter.ClearLastEvent();

            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
            logger.Error(exception, "Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            Assert.AreEqual(typeof(CommonLoggingSerilogTests).FullName, adapter.LastEvent.Source.Name);
            Assert.AreEqual("Processed { Latitude: 25, Longitude: 134 } in 034 ms.", adapter.LastEvent.RenderedMessage);
            Assert.AreSame(exception, adapter.LastEvent.Exception);
        }
    }
}
