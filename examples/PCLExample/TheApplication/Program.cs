using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using ThePortableProject;

namespace TheApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger<Program>();

            var logManager = new LogManager();

            var theClassThatLogs = new PortableClassWithILogDependency(logger);
            theClassThatLogs.SomeMethodThatLogs("I am the message from the ILog-dependent class");

            var otherClassThatLogs = new PortableClassWithILogManagerDependency(logManager);
            otherClassThatLogs.SomeMethodThatLogs("I am the message from the ILogManager-dependent class");

            Console.ReadKey();
        }
    }
}
