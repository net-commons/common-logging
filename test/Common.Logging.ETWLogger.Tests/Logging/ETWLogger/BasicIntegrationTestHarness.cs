using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Common.Logging.ETW;
using NUnit.Framework;
using Rhino.Mocks;

namespace Common.Logging.ETWLogger.Tests
{


    /*
     * The tests in this fixture have no asserts but can be used to produce output in the ETW subsystem for
     * review/inspection by actual human eyes by following these steps:
     * 
     * 1. Open the 'Diagnostics Events' window in Visual Studio (View > Other Windows > Diagnostic Events)
     * 2. In the 'Configure' menu in the Diagnostics Event window, add the following ETW providers:
     *     Common.Logging.ETWLogger
     *     Common.Logging.CustomTestEventSource
     * 3. Run the tests
     * 4. Observe the output in the Diagnostics Event window in Visual Studio
     * 
     */

    [TestFixture]
    public class BasicIntegrationTestHarness
    {
        [Test]
        public void BasicLoggingScenario()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(string.Empty);

            logger.Warn("This is a test message from ETW source!");
        }

        [Test]
        public void LoggingWithCustomEventSource()
        {
            var adapter = new ETWLoggerFactoryAdapter { EventSource = new CustomTestEventSource() };
            var logger = adapter.GetLogger(string.Empty);

            logger.Warn("This message should never appear in the ETW logs b/c its ignored by the custom EventSource for the sake of the test!");
        }

        [Test]
        public void LoggingWithException()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(string.Empty);

            logger.Debug("This is a test message from ETW source!", new Exception("I am the test exception"));
        }
    }
}
