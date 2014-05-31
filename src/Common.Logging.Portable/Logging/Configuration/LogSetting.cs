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

namespace Common.Logging.Configuration
{
    /// <summary>
    /// Container used to hold configuration information from config file.
    /// </summary>
    /// <author>Gilles Bayon</author>
    public class LogSetting
    {
        #region Fields

        private readonly Type _factoryAdapterType = null;
        private readonly NameValueCollection _properties = null;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param key="factoryAdapterType">
        /// The <see cref="ILoggerFactoryAdapter" /> type 
        /// that will be used for creating <see cref="ILog" />
        /// </param>
        /// <param key="properties">
        /// Additional user supplied properties that are passed to the 
        /// <paramref key="factoryAdapterType" />'s constructor.
        /// </param>
        public LogSetting(Type factoryAdapterType, NameValueCollection properties)
        {
            ArgUtils.AssertNotNull("factoryAdapterType", factoryAdapterType);
            ArgUtils.AssertIsAssignable<ILoggerFactoryAdapter>("factoryAdapterType", factoryAdapterType
                , "Type {0} does not implement {1}", factoryAdapterType.AssemblyQualifiedName, typeof(ILoggerFactoryAdapter).FullName);

            _factoryAdapterType = factoryAdapterType;
            _properties = properties;
        }

        /// <summary>
        /// The <see cref="ILoggerFactoryAdapter" /> type that will be used for creating <see cref="ILog" />
        /// instances.
        /// </summary>
        public Type FactoryAdapterType
        {
            get { return _factoryAdapterType; }
        }

        /// <summary>
        /// Additional user supplied properties that are passed to the <see cref="FactoryAdapterType" />'s constructor.
        /// </summary>
        public NameValueCollection Properties
        {
            get { return _properties; }
        }
    }
}