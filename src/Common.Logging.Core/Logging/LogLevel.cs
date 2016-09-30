#region License

/*
 * Copyright � 2002-2009 the original author or authors.
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

namespace Common.Logging
{
    /// <summary>
    /// The 7 possible logging levels
    /// </summary>
    /// <author>Gilles Bayon</author>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// All logging levels
        /// </summary>
        All = 0,
        /// <summary>
        /// A trace logging level
        /// </summary>
        Trace = 1,
        /// <summary>
        /// A performance logging level
        /// </summary>
        Performance = 2,
        /// <summary>
        /// A debug logging level
        /// </summary>
        Debug = 4,
        /// <summary>
        /// A info logging level
        /// </summary>
        Info = 8,
        /// <summary>
        /// A warn logging level
        /// </summary>
        Warn = 16,
        /// <summary>
        /// An error logging level
        /// </summary>
        Error = 32,
        /// <summary>
        /// A fatal logging level
        /// </summary>
        Fatal = 64,
        /// <summary>
        /// Do not log anything.
        /// </summary>
        Off = 128,
    }
}