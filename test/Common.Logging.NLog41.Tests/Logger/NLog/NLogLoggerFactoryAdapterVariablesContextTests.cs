using Common.Logging;
using Common.Logging.NLog;
using NUnit.Framework;

namespace Common.Logger.NLog
{
    [TestFixture]
    public class NLogLoggerFactoryAdapterVariablesContextTests
    {
        [Test]
        public void CheckGlobalVariablesSet()
        {
            var a = new NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);
            var testValue = new object();

            a.GetLogger(this.GetType()).GlobalVariablesContext.Set("TestKey", testValue);

            var actualValue = global::NLog.GlobalDiagnosticsContext.GetObject("TestKey");

            Assert.AreEqual(testValue, actualValue);
        }

        [Test]
        public void CheckThreadVariablesSet()
        {
            var a = new NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);

            a.GetLogger(this.GetType()).ThreadVariablesContext.Set("TestKey", "TestValue");

            var actualValue = global::NLog.MappedDiagnosticsContext.Get("TestKey");

            Assert.AreEqual("TestValue", actualValue);
        }
    }
}
