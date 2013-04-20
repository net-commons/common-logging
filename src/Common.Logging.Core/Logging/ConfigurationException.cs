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
#if PORTABLE
#else
using System.Runtime.Serialization;
#endif

namespace Common.Logging
{
    /// <summary>
    /// The exception that is thrown when a configuration system error has occurred with Common.Logging
    /// </summary>
    /// <author>Mark Pollack</author>
#if PORTABLE
    public class ConfigurationException : Exception
#else
    [Serializable]
    public class ConfigurationException : ApplicationException
#endif
    {
        #region Constructor (s) / Destructor

        /// <summary>Creates a new instance of the ObjectsException class.</summary>
        public ConfigurationException()
        {
        }

        /// <summary>
        /// Creates a new instance of the ConfigurationException class. with the specified message.
        /// </summary>
        /// <param key="message">
        /// A message about the exception.
        /// </param>
        public ConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of the ConfigurationException class with the specified message
        /// and root cause.
        /// </summary>
        /// <param key="message">
        /// A message about the exception.
        /// </param>
        /// <param key="rootCause">
        /// The root exception that is being wrapped.
        /// </param>
        public ConfigurationException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }

#if PORTABLE
#else
       /// <summary>
        /// Creates a new instance of the ConfigurationException class.
        /// </summary>
        /// <param key="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param key="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/>
        /// that contains contextual information about the source or destination.
        /// </param>
        protected ConfigurationException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        #endregion
    }
}