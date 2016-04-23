using System.Collections.Generic;
using System.Collections.Specialized;
using Common.Logging.Configuration;
using Common.Logging.Log4Net;
using Common.Logging.NLog;
using Common.Logging.Simple;
using NUnit.Framework;
using NameValueCollection = Common.Logging.Configuration.NameValueCollection;

namespace Common.Logging.MultipleLogger.Tests.Logging.MultipleLogger
{
    [TestFixture]
    public class MultipleLoggerGlobalVariableContextTests
    {
        private Log4NetLoggerFactoryAdapter _log4NetLoggerFactoryAdapter;
        private NLogLoggerFactoryAdapter _nLogLoggerFactoryAdapter;
        private MultiLoggerFactoryAdapter _multipleLoggerFactoryAdapter;


        [SetUp]
        public void TestSetUp()
        {
            _log4NetLoggerFactoryAdapter = new Log4NetLoggerFactoryAdapter(new NameValueCollection());
            _nLogLoggerFactoryAdapter = new NLogLoggerFactoryAdapter(new NameValueCollection());
            _multipleLoggerFactoryAdapter = new MultiLoggerFactoryAdapter();

            _multipleLoggerFactoryAdapter.LoggerFactoryAdapters.Add(_log4NetLoggerFactoryAdapter);
            _multipleLoggerFactoryAdapter.LoggerFactoryAdapters.Add(_nLogLoggerFactoryAdapter);

            //these tests will only work if all of the loggers actually *support* VariablesContext with other than the No-Op 'placeholder'
            Assume.That(_log4NetLoggerFactoryAdapter.GetLogger(typeof(MultipleLoggerGlobalVariableContextTests)).GlobalVariablesContext, Is.Not.InstanceOf<NoOpVariablesContext>());
            Assume.That(_nLogLoggerFactoryAdapter.GetLogger(typeof(MultipleLoggerGlobalVariableContextTests)).GlobalVariablesContext, Is.Not.InstanceOf<NoOpVariablesContext>());
        }


        [Test]
        public void WhenValueHasBeenSet_CanGetValue()
        {
            LogManager.Adapter = _multipleLoggerFactoryAdapter;
            var multiLogger = LogManager.GetLogger<MultipleLoggerGlobalVariableContextTests>();

            const string EXPECTED_VALUE = "myValue";
            const string KEY = "myKey";

            multiLogger.GlobalVariablesContext.Set(KEY, EXPECTED_VALUE);

            var actualValue = multiLogger.GlobalVariablesContext.Get(KEY);

            Assert.That((string)actualValue == EXPECTED_VALUE);
        }


        [Test]
        public void WhenValueHasNotBeenSet_CanReturnNoValue()
        {
            LogManager.Adapter = _multipleLoggerFactoryAdapter;
            var multiLogger = LogManager.GetLogger<MultipleLoggerGlobalVariableContextTests>();

            var actualValue = multiLogger.GlobalVariablesContext.Get("KEY_NEVER_SET");

            //NOTE: the impl. for NLog returns string.Empty for no match so getting that back = PASS
            Assert.That((string) actualValue == string.Empty);
        }

        [Test]
        public void SettingValueResultsInValueSetOnAllMemberLoggers()
        {
            LogManager.Adapter = _multipleLoggerFactoryAdapter;
            var multiLogger = LogManager.GetLogger<MultipleLoggerGlobalVariableContextTests>();

            const string EXPECTED_VALUE = "myValue";
            const string KEY = "myKey";

            multiLogger.GlobalVariablesContext.Set(KEY, EXPECTED_VALUE);


            LogManager.Adapter = _log4NetLoggerFactoryAdapter;
            var log4NetLogger = LogManager.GetLogger<MultipleLoggerGlobalVariableContextTests>();
            var log4NetActualValue = log4NetLogger.GlobalVariablesContext.Get(KEY);

            LogManager.Adapter = _nLogLoggerFactoryAdapter;
            var nLogLogger = LogManager.GetLogger<MultipleLoggerGlobalVariableContextTests>();
            var nLogActualValue = nLogLogger.GlobalVariablesContext.Get(KEY);

            
            Assert.That((string)log4NetActualValue == EXPECTED_VALUE);
            Assert.That((string)nLogActualValue == EXPECTED_VALUE);
        }

    }
}