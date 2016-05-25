using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging.ETW;
using NUnit.Framework;

namespace Common.Logging.ETWLogger.Tests
{
    [TestFixture]
    public class TestHarness
    {
        [Test]
        public void MyMethod()
        {
            var adapter = new ETWLoggerFactoryAdapter();
            var logger = adapter.GetLogger(typeof(TestHarness));

            logger.Warn("This is a test message from ETW source!");
        }
    }
}
