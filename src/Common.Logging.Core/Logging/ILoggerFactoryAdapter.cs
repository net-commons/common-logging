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

namespace Common.Logging
{
	/// <summary>
	/// LoggerFactoryAdapter interface is used internally by LogManager
	/// Only developers wishing to write new Common.Logging adapters need to
	/// worry about this interface.
	/// </summary>
    /// <author>Gilles Bayon</author>
	public interface ILoggerFactoryAdapter 
	{

        /// <summary>
        /// Get a ILog instance by type.
        /// </summary>
        /// <param key="type">The type to use for the logger</param>
        /// <returns></returns>
		ILog GetLogger( Type type );

        /// <summary>
        /// Get a ILog instance by key.
        /// </summary>
        /// <param key="key">The key of the logger</param>
        /// <returns></returns>
		ILog GetLogger( string name );	

	}
}
