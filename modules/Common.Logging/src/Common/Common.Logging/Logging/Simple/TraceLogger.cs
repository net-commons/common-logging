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

using System;
using System.Diagnostics;
using System.Text;

namespace Common.Logging.Simple
{
	/// <summary>
	/// Logger sending everything to the trace output stream using <see cref="System.Diagnostics.Trace"/>.
	/// </summary>
	/// <author>Gilles Bayon</author>
	/// <author>Erich Eichinger</author>
    [Serializable]
    public class TraceLogger: AbstractSimpleLogger
	{
        /// <summary>
        /// Used to defer message formatting until it is really needed.
        /// </summary>
        /// <remarks>
        /// This class also improves performance when multiple 
        /// <see cref="TraceListener"/>s are configured.
        /// </remarks>
        private class FormatOutputMessage
        {
            private readonly TraceLogger outer;
            private readonly LogLevel level;
            private readonly object message;
            private readonly Exception ex;

            public FormatOutputMessage(TraceLogger outer, LogLevel level, object message, Exception ex)
            {
                this.outer = outer;
                this.level = level;
                this.message = message;
                this.ex = ex;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                outer.FormatOutput(sb, level, message, ex);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Creates a new TraceLogger instance.
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="logLevel"></param>
        /// <param name="showDateTime">Include the current time in the log message </param>
        /// <param name="showLogName">Include the instance name in the log message</param>
        /// <param name="dateTimeFormat">The date and time format to use in the log message </param>
	    public TraceLogger(string logName, LogLevel logLevel, bool showDateTime, bool showLogName, string dateTimeFormat) 
            : base(logName, logLevel, showDateTime, showLogName, dateTimeFormat)
	    {
	    }

		/// <summary>
		/// Do the actual logging.
		/// </summary>
		/// <param name="level"></param>
		/// <param name="message"></param>
		/// <param name="e"></param>
		protected override void WriteInternal( LogLevel level, object message, Exception e )
		{
			// Print to the appropriate destination
			System.Diagnostics.Trace.WriteLine( new FormatOutputMessage(this, level, message, e) );			
		}
	}
}

