using System;
using NUnit.Framework;

namespace Common
{
    internal interface IWriteHandler
    {
        void PerformanceTestTarget(int z, object message, Exception ex);
    }

    [TestFixture, Explicit]
    public class MethodInvokePerformanceTests : IWriteHandler
    {
        private delegate void WriteHandler(int x, object message, Exception ex);

        [Test]
        public void DelegatePerformanceTest()
        {
            int runs = 5000000;

            StopWatch sw;
            sw = new StopWatch(); 
            using(sw.Start("Time:{0}"))
            {
                for (int i = 0; i < runs; i++)
                {
                    PerformanceTestTarget(1, null, null);
                }
            }
//            Trace.WriteLine( string.Format("Time:{0}", sw.Elapsed.TotalSeconds) );

            WriteHandler writeHandler = new WriteHandler(PerformanceTestTarget);
            using(sw.Start("Time:{0}"))
            {
                for (int i = 0; i < runs; i++)
                {
                    writeHandler(1, null, null);
                }
            }
//            Trace.WriteLine(string.Format("Time:{0}", sw.Elapsed.TotalSeconds));
 
            IWriteHandler writeHandler2 = this;
            using(sw.Start("Time:{0}"))
            {
                for (int i = 0; i < runs; i++)
                {
                    writeHandler2.PerformanceTestTarget(1, null, null);
                }
            }
//            Trace.WriteLine(string.Format("Time:{0}", sw.Elapsed.TotalSeconds));
        }

        public void PerformanceTestTarget(int z, object message, Exception ex)
        {
            double x = 2.1;
            for(int i=0;i<10;i++)
            {
                x = x * x * x;
            }
        }
    }
}
