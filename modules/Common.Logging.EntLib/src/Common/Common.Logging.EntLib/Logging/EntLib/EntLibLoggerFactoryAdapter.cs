#region License

/*
 * Copyright 2002-2007 the original author or authors.
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

namespace Common.Logging.EntLib
{
    /// <summary>
    /// This is 
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <author>Mark Pollack</author>
    /// <version>$Id:$</version>
    public class EntLibLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        public EntLibLoggerFactoryAdapter()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>passed in values are not used, configuration is external to EntLib logging API</remarks>
        /// <param name="properties">The properties.</param>
        public EntLibLoggerFactoryAdapter(NameValueCollection properties)
        {
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name to use as the category.</param>
        /// <returns></returns>
        public ILog GetLogger(string name)
        {
            return new EntLibLogger(name);
        }
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="type">The type property FullName is used as the category.</param>
        /// <returns></returns>
        public ILog GetLogger(Type type)
        {
            return new EntLibLogger(type.FullName);
        }


    }
}