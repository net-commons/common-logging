#region License

/*
 * Copyright 2002-2009 the original author or authors.
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

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Holds configuration settings for <see cref="EntLibLogger"/>s.
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class EntLibLoggerSettings
    {
        /// <summary>
        /// the default priority used for logging. 
        /// </summary>
        public static readonly int DEFAULTPRIORITY = -1;

        /// <summary>
        /// the default <see cref="exceptionFormat"/> used for formatting error message
        /// </summary>
        /// <remarks>
        /// "Exception[ message = $(exception.message), source = $(exception.source), targetsite = $(exception.targetsite), stacktrace = $(exception.stacktrace) ]"
        /// </remarks>
        public static readonly string DEFAULTEXCEPTIONFORMAT = "Exception[ message = $(exception.message), source = $(exception.source), targetsite = $(exception.targetsite), stacktrace = $(exception.stacktrace) ]";

        /// <summary>
        /// the default priority to be used.
        /// </summary>
        public readonly int priority = DEFAULTPRIORITY;
        //format like nlog is better? - i.e. ${exception:format=message,stacktrace:separator=, }
        /// <summary>
        /// the exception format to be used.
        /// </summary>
        public readonly string exceptionFormat = DEFAULTEXCEPTIONFORMAT;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EntLibLoggerSettings(int defaultPriority, string exceptionFormat)
        {
            this.priority = defaultPriority;
            this.exceptionFormat = exceptionFormat;
        }
    }
}