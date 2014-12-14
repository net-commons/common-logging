using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;

namespace ThePortableProject
{
    public class PortableClassWithILogDependency
    {
        private readonly ILog _logger;

        public PortableClassWithILogDependency(ILog logger)
        {
            _logger = logger;
        }

        public void SomeMethodThatLogs(string message)
        {
            _logger.Info(message);
        }
    }
}
