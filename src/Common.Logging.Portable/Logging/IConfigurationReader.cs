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

#if PORTABLE
#else
using System.Configuration;
#endif

namespace Common.Logging
{

    /// <summary>
    /// Interface for basic operations to read .NET application configuration information.
    /// </summary>
    /// <remarks>Provides a simple abstraction to handle BCL API differences between .NET 1.x and 2.0. Also
    /// useful for testing scenarios.</remarks>
    /// <author>Mark Pollack</author>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Parses the configuration section and returns the resulting object.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Primary purpose of this method is to allow us to parse and 
        /// load configuration sections using the same API regardless
        /// of the .NET framework version.
        /// </p>
        /// 
        /// See also <c>System.Configuration.ConfigurationManager</c>
        /// </remarks>
        /// <param key="sectionName">Name of the configuration section.</param>
        /// <returns>Object created by a corresponding <see cref="IConfigurationSectionHandler"/>.</returns>
        object GetSection(string sectionName);
    }
}
