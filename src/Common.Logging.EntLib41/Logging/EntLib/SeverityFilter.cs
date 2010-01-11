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

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using Common.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Filters <see cref="LogEntry"/>s by severity (see <see cref="TraceEventType"/>).
    /// </summary>
    /// <author>Erich Eichinger</author>
    [ConfigurationElementType(typeof(CustomLogFilterData))]
    public class SeverityFilter : LogFilter
    {
        private int severityMask = Int32.MaxValue;

        /// <summary>
        /// Bitmask to identify severity levels that should be logged.
        /// </summary>
        public int SeverityMask
        {
            get { return severityMask; }
            set { severityMask = value; }
        }

        /// <summary>
        /// Creates a new filter instance
        /// </summary>
        public SeverityFilter(string name, int severityMask)
            : base(name)
        {
            this.severityMask = severityMask;
        }

        /// <summary>
        /// Creates a new filter instance
        /// </summary>
        public SeverityFilter(string name, TraceEventType severityMask)
            : this(name, (int)severityMask)
        {}

        /// <summary>
        /// Creates a new filter instance
        /// </summary>
        public SeverityFilter(NameValueCollection attributes)
            : base((attributes != null && attributes["name"] != null) ? attributes["name"] : "Severity Filter")
        {
            this.severityMask = ArgUtils.TryParse(this.severityMask
                                                               , ArgUtils.GetValue(attributes, "SeverityMask"));
        }

        /// <summary>
        /// Check, if <paramref name="log"/> severity matches <see cref="severityMask"/>.
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override bool Filter(LogEntry log)
        {
            return ShouldLog(log.Severity);
        }

        /// <summary>
        /// Checks, whether the specified severity is allowed to log.
        /// </summary>
        public bool ShouldLog(TraceEventType severity)
        {
            int evSeverity = (int)severity;

            return ((evSeverity & severityMask) == evSeverity);
        }
    }
}
