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
using System.Collections.Specialized;
using System.Diagnostics;

namespace Common.Logging.Configuration
{
    /// <summary>
    /// Various utility methods for using during factory and logger instance configuration
    /// </summary>
    /// <author>Erich Eichinger</author>
    public sealed class ConfigurationHelper
    {
        /// <summary>
        /// Do not instantiate
        /// </summary>
        private ConfigurationHelper()
        {}

        /// <summary>
        /// Retrieves the named value from the specified <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="values">may be null</param>
        /// <param name="name">the value's key</param>
        /// <returns>if <paramref name="values"/> is not null, the value returned by values[name]. <c>null</c> otherwise.</returns>
        public static string GetValue(NameValueCollection values, string name)
        {
            return GetValue(values, name, null);
        }

        /// <summary>
        /// Retrieves the named value from the specified <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="values">may be null</param>
        /// <param name="name">the value's key</param>
        /// <param name="defaultValue">the default value, if not found</param>
        /// <returns>if <paramref name="values"/> is not null, the value returned by values[name]. <c>null</c> otherwise.</returns>
        public static string GetValue(NameValueCollection values, string name, string defaultValue)
        {
            if (values != null)
            {
                foreach(string key in values.AllKeys)
                {
                    if (string.Compare(name, key, true) == 0)
                    {
                        return values[name];
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the first nonnull, nonempty string among its arguments.
        /// </summary>
        public static string Coalesce(params string[] stringValues)
        {
            if (stringValues == null || stringValues.Length == 0)
            {
                return null;
            }

            for (int i = 0; i < stringValues.Length; i++)
            {
                string stringValue = stringValues[i];
                if (stringValue != null && stringValue.Length > 0)
                {
                    return stringValue;
                }                
            }
            return null;
        }

        /// <summary>
        /// Tries parsing <paramref name="stringValue"/> into an enum of the type of <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref name="defaultValue"/> otherwise.</returns>
        public static object TryParseEnum(Enum defaultValue, string stringValue)
        {
            object result = defaultValue;
            if (stringValue == null || stringValue.Length == 0)
            {
                return defaultValue;
            }
            try
            {
                result = Enum.Parse(defaultValue.GetType(), stringValue, true);
            }
            catch
            {
                Trace.WriteLine(string.Format("WARN: failed converting value '{0}' to enum type '{1}'", stringValue, defaultValue.GetType().FullName));
            }
            return result;
        }

        /// <summary>
        /// Tries parsing <paramref name="stringValue"/> into a <see cref="bool"/>.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref name="defaultValue"/> otherwise.</returns>
        public static bool TryParseBoolean(bool defaultValue, string stringValue)
        {
            bool result = defaultValue;
            if (stringValue == null || stringValue.Length == 0)
            {
                return defaultValue;
            }
            try
            {
                result = bool.Parse(stringValue);
            }
            catch
            {
                Trace.WriteLine(string.Format("WARN: failed converting value '{0}' to boolean", stringValue));
            }
            return result;
        }

        /// <summary>
        /// Tries parsing <paramref name="stringValue"/> into an <see cref="int"/>.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref name="defaultValue"/> otherwise.</returns>
        public static int TryParseInt(int defaultValue, string stringValue)
        {
            int result = defaultValue;
            if (stringValue == null || stringValue.Length == 0)
            {
                return defaultValue;
            }
            try
            {
                result = Convert.ToInt32(stringValue);
            }
            catch
            {
                Trace.WriteLine(string.Format("WARN: failed converting value '{0}' to int", stringValue));
            }
            return result;
        }
    }
}