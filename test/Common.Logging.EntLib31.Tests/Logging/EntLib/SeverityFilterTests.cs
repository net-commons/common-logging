using System;
using System.Collections.Specialized;
using System.Diagnostics;
using NUnit.Framework;

namespace Common.Logging.EntLib
{
    [TestFixture]
    public class SeverityFilterTests
    {
        [Test]
        public void DefaultSettings()
        {
            SeverityFilter sf = new SeverityFilter(null);
            Assert.AreEqual("Severity Filter", sf.Name);
            Assert.AreEqual(Int32.MaxValue, sf.SeverityMask);            
        }

        [Test]
        public void SetsProperties()
        {
            SeverityFilter sf = new SeverityFilter("name", 10);
            Assert.AreEqual("name", sf.Name);
            Assert.AreEqual(10, sf.SeverityMask);

            NameValueCollection props = new NameValueCollection();
            props["Name"] = "name";
            props["SeverityMask"] = "10";
            sf = new SeverityFilter(props);
            Assert.AreEqual("name", sf.Name);
            Assert.AreEqual(10, sf.SeverityMask);
        }

        [Test]
        public void FiltersByMask()
        {
            SeverityFilter sf = new SeverityFilter("name", 6);
            Assert.IsTrue(sf.ShouldLog((TraceEventType) 0));
            Assert.IsFalse(sf.ShouldLog((TraceEventType) 1));
            Assert.IsTrue(sf.ShouldLog((TraceEventType) 2));
            Assert.IsTrue(sf.ShouldLog((TraceEventType) 4));
            Assert.IsFalse(sf.ShouldLog((TraceEventType) 7));
            Assert.IsFalse(sf.ShouldLog((TraceEventType) 255));
        }
    }
}
