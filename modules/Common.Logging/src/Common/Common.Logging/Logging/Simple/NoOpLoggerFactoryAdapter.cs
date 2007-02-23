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
using System.Collections.Specialized;

namespace Common.Logging.Simple
{
	/// <summary>
    /// Factory for creating <see cref="ILog" /> instances that silently ignores
    /// logging requests.
	/// </summary>
    /// <author>Gilles Bayon</author>
    /// <version>$Id: NoOpLoggerFactoryAdapter.cs,v 1.1 2006/11/13 07:17:55 markpollack Exp $</version>
	public sealed class NoOpLoggerFactoryAdapter : ILoggerFactoryAdapter
	{
		private ILog _nopLogger = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoOpLoggerFactoryAdapter()
        {
            _nopLogger = new NoOpLogger();
        }

		/// <summary>
		/// Constructor
		/// </summary>
        public NoOpLoggerFactoryAdapter(NameValueCollection properties)
		{
			_nopLogger = new NoOpLogger();
		}

		#region ILoggerFactoryAdapter Members

		/// <summary>
		/// Get a ILog instance by type 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ILog GetLogger(Type type)
		{
			return _nopLogger;
		}

		/// <summary>
		/// Get a ILog instance by type name 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		ILog ILoggerFactoryAdapter.GetLogger(string name)
		{
			return _nopLogger;

		}

		#endregion
	}
}
