#region License

/*
 * Copyright © 2002-2006 the original author or authors.
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

using System.Collections.Specialized;
using Common.Logging;
using Common.Logging.Simple;
using NUnit.Framework;

namespace Spring.Logging.Simple
{
    /// <summary>
    /// Exercises the NoOpLogger implementation
    /// </summary>
    public class NoOpLoggerTests : AbstractLogTest
    {
        public override ILog LogObject
        {
            get
            {
                NameValueCollection properties = new NameValueCollection();
                properties["showDateTime"] = "true";

                // set Adapter
                LogManager.Adapter = new NoOpLoggerFactoryAdapter(properties);
                return LogManager.GetLogger("NoOpLogger");
            }
        }
        
        [Test]
        public void DefaultSettings()
        {
            CheckLog(LogObject);
        }
        
        private void CheckLog(ILog log)
        {
            Assert.IsNotNull(log);
            Assert.IsInstanceOfType(typeof(NoOpLogger), log);

            // Can we call level checkers with no exceptions?
            Assert.IsFalse(log.IsDebugEnabled);
            Assert.IsFalse(log.IsInfoEnabled);
            Assert.IsFalse(log.IsWarnEnabled);
            Assert.IsFalse(log.IsErrorEnabled);
            Assert.IsFalse(log.IsFatalEnabled);
        }
    }
}
