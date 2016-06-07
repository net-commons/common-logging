// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Configuration;
using Common.Logging.Serilog;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.CommonLogging() extension method to <see cref="LoggerConfiguration"/>.
    /// This is required for XML Configuration to work in Common.Logging
    /// </summary>
    /// <author>Aaron Mell</author>
    public static class LoggerConfigurationEventLogExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events to the Windows event log.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration CommonLogging(this LoggerSinkConfiguration loggerConfiguration)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");


            return loggerConfiguration.Sink(new CommonLoggingSink());
        }
    }
}