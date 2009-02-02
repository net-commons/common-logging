#if NET_3_0

using System;
using System.Globalization;
using Common.Logging;
using NUnit.Framework;

namespace Common
{
    /// <summary>
	/// Summary description for Class1.
	/// </summary>
	[TestFixture]
	public class Class1
	{
        [Test]
		public void CanCompile()
		{
            ILog log = LogManager.GetCurrentClassLogger();
            log.Trace(CultureInfo.InvariantCulture,  m => m("test {0}", "test"), new Exception());
		}
	}
}

#endif