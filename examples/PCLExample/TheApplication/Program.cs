using System;
using System.Collections.Generic;
using System.Linq;
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
            ILogManager manager = new LogManager();

            var logger = manager.GetLogger<Program>();

            var theClassThatLogs = new ThePortableClass(logger);
            theClassThatLogs.SomeMethodThatLogs("I am the message!");

            Console.ReadKey();
        }
    }
}
