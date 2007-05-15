using System;
using System.Reflection;
using NUnit.Framework;

namespace Common.Logging
{
    /// <summary>
    /// Test that exceptions declared in Common.Logging meet coding standards.
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <version>$Id:$</version>    
    [TestFixture]
    public class LoggingExceptionTests : ExceptionsTest
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            AssemblyToCheck = Assembly.GetAssembly(typeof (ConfigurationException));
        }

        [Test]
        public void CreateWithNestedException()
        {
            LoggingException le = new LoggingException(new ArithmeticException());
        }
    }
}