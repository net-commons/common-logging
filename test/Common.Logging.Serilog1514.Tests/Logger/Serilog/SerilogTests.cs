#region License

/*
 * Copyright © 2002-2007 the original author or authors.
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

using System.Security;
using Common.TestUtil;
using NUnit.Framework;

namespace Common.Logging.Serilog
{
    /// <summary>
    /// Test for the Serilog implementation of ILog 
    /// </summary>
    /// <author>Aaron Mell</author>
    [TestFixture]
    public class SerilogTests : ILogTestsBase
    {
        protected override ILoggerFactoryAdapter GetLoggerFactoryAdapter()
        {
            return new SerilogLoggerFactoryAdapter(new Common.Logging.Configuration.NameValueCollection());
        }

        /// <summary>
        /// Serilog lacks <see cref="AllowPartiallyTrustedCallersAttribute"/> 
        /// and therefore needs full trust environments.
        /// </summary>
        protected override string CompliantTrustLevelName
        {
            get
            {
                return SecurityTemplate.PERMISSIONSET_FULLTRUST;
            }
        }

        [Test]
        public void AssertDefaultSettings()
        {
            ILog log = LogManager.GetLogger<SerilogTests>();
            Assert.IsNotNull(log);
            Assert.IsInstanceOf<SerilogLogger>(log);

            // Can we call level checkers with no exceptions?
            Assert.IsFalse(log.IsTraceEnabled);
            Assert.IsFalse(log.IsDebugEnabled);
            Assert.IsFalse(log.IsInfoEnabled);
            Assert.IsFalse(log.IsWarnEnabled);
            Assert.IsFalse(log.IsErrorEnabled);
            Assert.IsFalse(log.IsFatalEnabled);
        }
    }
}