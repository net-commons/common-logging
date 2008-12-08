using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace LambdaCallbackEvaluation
{
    class CommonLoggingBinaryCompatibilityTest
    {
        public static void Main()
        {
            Common.Logging.LogManager.Adapter = new TraceLoggerFactoryAdapter();

            CommonLoggingBinaryCompatibilityTest tests = new CommonLoggingBinaryCompatibilityTest();
            tests.Run();
        }

        private ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private object message = "ObjectMessage";
        private Exception exception = new Exception();

        public void Run()
        {
            log.Trace(message);
            log.Trace(message, exception);
            log.Debug(message);
            log.Debug(message, exception);
            log.Info(message);
            log.Info(message, exception);
            log.Warn(message);
            log.Warn(message, exception);
            log.Error(message);
            log.Error(message, exception);
            log.Fatal(message);
            log.Fatal(message, exception);            
        }
    }
}
