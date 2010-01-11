#if NET_3_0

using Common.Logging;

namespace Common
{
    class AnotherClass
    {
        public void CanCompile()
        {
            ILog log = LogManager.GetCurrentClassLogger();
            log.Trace(m=> m("test {0}", "test"));
        }
    }
}

#endif