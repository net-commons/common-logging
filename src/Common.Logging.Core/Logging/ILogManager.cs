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
using System.Runtime.CompilerServices;

namespace Common.Logging
{
    /// <summary>
    /// Interface for LogManager
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// The key of the default configuration section to read settings from.
        /// </summary>
        /// <remarks>
        /// You can always change the source of your configuration settings by setting another <see cref="IConfigurationReader"/> instance
        /// on <see cref="ConfigurationReader"/>.
        /// </remarks>
        string COMMON_LOGGING_SECTION { get; }

        /// <summary>
        /// Reset the <see cref="Common.Logging" /> infrastructure to its default settings. This means, that configuration settings
        /// will be re-read from section <c>&lt;common/logging&gt;</c> of your <c>app.config</c>.
        /// </summary>
        /// <remarks>
        /// This is mainly used for unit testing, you wouldn't normally use this in your applications.<br/>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected. 
        /// Resetting LogManager only affects new instances being handed out.
        /// </remarks>
        void Reset();

        /// <summary>
        /// Reset the <see cref="Common.Logging" /> infrastructure to its default settings. This means, that configuration settings
        /// will be re-read from section <c>&lt;common/logging&gt;</c> of your <c>app.config</c>.
        /// </summary>
        /// <remarks>
        /// This is mainly used for unit testing, you wouldn't normally use this in your applications.<br/>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected. 
        /// Resetting LogManager only affects new instances being handed out.
        /// </remarks>
        /// <param name="reader">
        /// the <see cref="IConfigurationReader"/> instance to obtain settings for 
        /// re-initializing the LogManager.
        /// </param>
        void Reset(IConfigurationReader reader);

        /// <summary>
        /// Gets the configuration reader used to initialize the LogManager.
        /// </summary>
        /// <remarks>Primarily used for testing purposes but maybe useful to obtain configuration
        /// information from some place other than the .NET application configuration file.</remarks>
        /// <value>The configuration reader.</value>
        IConfigurationReader ConfigurationReader { get; }

        /// <summary>
        /// Gets or sets the adapter.
        /// </summary>
        /// <value>The adapter.</value>
        ILoggerFactoryAdapter Adapter { get; set; }

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the type of the calling class.
        /// </summary>
        /// <remarks>
        /// This method needs to inspect the StackTrace in order to determine the calling 
        /// class. This of course comes with a performance penalty, thus you shouldn't call it too
        /// often in your application.
        /// </remarks>
        /// <seealso cref="GetLogger(Type)"/>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        [Obsolete("Null-Reference Exception when dealing with Dynamic Types, Prefer instead one of the LogManager.GetLogger(...) variants.")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        ILog GetCurrentClassLogger();

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified type.
        /// </summary>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        ILog GetLogger<T>();

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        ILog GetLogger(Type type);

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(string)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        ILog GetLogger(string key);
    }
}