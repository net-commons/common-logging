#region License

/*
 * Copyright © 2002-2009 the original author or authors.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.NLog;
using NUnit.Framework;

namespace Common.Logger.NLog
{
    [TestFixture]
    public class NLogLoggerFactoryAdapterVariablesContextTests
    {
        [Test]
        public void CheckGlobalVariablesSet()
        {
            var a = new NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);
            var testValue = new object();

            a.GetLogger(this.GetType()).GlobalVariablesContext.Set("TestKey", testValue);

            var actualValue = global::NLog.GlobalDiagnosticsContext.GetObject("TestKey");

            Assert.AreEqual(testValue, actualValue);
        }

        [Test]
        public void CheckThreadVariablesSet()
        {
            var a = new NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);

            a.GetLogger(this.GetType()).ThreadVariablesContext.Set("TestKey", "TestValue");

            var actualValue = global::NLog.MappedDiagnosticsContext.Get("TestKey");

            Assert.AreEqual("TestValue", actualValue);
        }

	    [Test]
	    public void CheckNestedThreadVariablesSet()
	    {
		    var a = new NLogLoggerFactoryAdapter((NameValueCollection) null);

		    var hasItems = a.GetLogger(this.GetType()).NestedThreadVariablesContext.HasItems;
		    Assert.AreEqual(false, hasItems);

		    a.GetLogger(this.GetType()).NestedThreadVariablesContext.Push("TestValue1");

		    hasItems = a.GetLogger(this.GetType()).NestedThreadVariablesContext.HasItems;
		    Assert.AreEqual(true, hasItems);

		    a.GetLogger(this.GetType()).NestedThreadVariablesContext.Push("TestValue2");

		    int depth = global::NLog.NestedDiagnosticsContext.GetAllMessages().Length;
		    Assert.AreEqual(2, depth);

		    var actualValue = a.GetLogger(this.GetType()).NestedThreadVariablesContext.Pop();
		    Assert.AreEqual("TestValue2", actualValue);

		    actualValue = a.GetLogger(this.GetType()).NestedThreadVariablesContext.Pop();
		    Assert.AreEqual("TestValue1", actualValue);

		    hasItems = a.GetLogger(this.GetType()).NestedThreadVariablesContext.HasItems;
		    Assert.AreEqual(false, hasItems);

		    depth = global::NLog.NestedDiagnosticsContext.GetAllMessages().Length;
		    Assert.AreEqual(0, depth);
	    }

    }
}
