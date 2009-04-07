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
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Common.Logging.Configuration
{
    /// <summary>
    /// Various utility methods for using during factory and logger instance configuration
    /// </summary>
    /// <author>Erich Eichinger</author>
    public static class ArgUtils
    {
        /// <summary>
        /// A delegate converting a string representation into the target type
        /// </summary>
        public delegate T ParseHandler<T>(string strValue);

        private static readonly Hashtable s_parsers;

        /// <summary>
        /// Initialize all members before any of this class' methods can be accessed (avoids beforeFieldInit)
        /// </summary>
        static ArgUtils()
        {
            s_parsers = new Hashtable();
            RegisterTypeParser<bool>(delegate(string s) { return Convert.ToBoolean(s); });
            RegisterTypeParser<short>(delegate(string s) { return Convert.ToInt16(s); });
            RegisterTypeParser<int>(delegate(string s) { return Convert.ToInt32(s); });
            RegisterTypeParser<long>(delegate(string s) { return Convert.ToInt64(s); });
            RegisterTypeParser<float>(delegate(string s) { return Convert.ToSingle(s); });
            RegisterTypeParser<double>(delegate(string s) { return Convert.ToDouble(s); });
            RegisterTypeParser<decimal>(delegate(string s) { return Convert.ToDecimal(s); });
        }

        /// <summary>
        /// Adds the parser to the list of known type parsers.
        /// </summary>
        /// <remarks>
        /// .NET intrinsic types are pre-registerd: short, int, long, float, double, decimal, bool
        /// </remarks>
        public static void RegisterTypeParser<T>(ParseHandler<T> parser)
        {
            s_parsers[typeof(T)] = parser;
        }

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
                foreach (string key in values.AllKeys)
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
        /// Returns the first nonnull, nonempty value among its arguments.
        /// </summary>
        /// <remarks>
        /// Returns <c>null</c>, if the initial list was null or empty.
        /// </remarks>
        /// <seealso cref="Coalesce{T}"/>
        public static string Coalesce(params string[] values)
        {
            return Coalesce(delegate(string v) { return !string.IsNullOrEmpty(v); }, values);
        }

        /// <summary>
        /// Returns the first nonnull, nonempty value among its arguments.
        /// </summary>
        /// <remarks>
        /// Also 
        /// </remarks>
        public static T Coalesce<T>(Predicate<T> predicate, params T[] values) where T : class
        {
            if (values == null || values.Length == 0)
            {
                return null;
            }

            if (predicate == null)
            {
                predicate = delegate(T v) { return v != null; };
            }

            for (int i = 0; i < values.Length; i++)
            {
                T val = values[i];
                if (predicate(val))
                {
                    return val;
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
        public static T TryParseEnum<T>(T defaultValue, string stringValue) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException(string.Format("Type '{0}' is not an enum type", typeof(T).FullName));
            }

            T result = defaultValue;
            if (string.IsNullOrEmpty(stringValue))
            {
                return defaultValue;
            }
            try
            {
                result = (T)Enum.Parse(typeof(T), stringValue, true);
            }
            catch
            {
                Trace.WriteLine(string.Format("WARN: failed converting value '{0}' to enum type '{1}'", stringValue, defaultValue.GetType().FullName));
            }
            return result;
        }

        /// <summary>
        /// Tries parsing <paramref name="stringValue"/> into the specified return type.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref name="defaultValue"/> otherwise.</returns>
        public static T TryParse<T>(T defaultValue, string stringValue)
        {
            T result = defaultValue;
            if (string.IsNullOrEmpty(stringValue))
            {
                return defaultValue;
            }

            ParseHandler<T> parser = s_parsers[typeof(T)] as ParseHandler<T>;
            if (parser == null)
            {
                throw new ArgumentException(string.Format("There is no parser registered for type {0}", typeof(T).FullName));
            }

            try
            {
                result = parser(stringValue);
            }
            catch
            {
                Trace.WriteLine(string.Format("WARN: failed converting value '{0}' to type '{1}' - returning default '{2}'", stringValue, typeof(T).FullName, result));
            }
            return result;
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if <paramref name="val"/> is <c>null</c>.
        /// </summary>
        public static T AssertNotNull<T>(string paramName, T val) where T : class
        {
            if (ReferenceEquals(val, null))
            {
                throw new ArgumentNullException(paramName);
            }
            return val;
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if <paramref name="val"/> is <c>null</c>.
        /// </summary>
        public static T AssertNotNull<T>(string paramName, T val, string messageFormat, params object[] args) where T : class
        {
            if (ReferenceEquals(val, null))
            {
                throw new ArgumentNullException(paramName, string.Format(messageFormat, args));
            }
            return val;
        }

        /// <summary>
        /// Throws a <see cref="ArgumentOutOfRangeException"/> if an object of type <paramref name="valType"/> is not
        /// assignable to type <typeparam name="T"></typeparam>.
        /// </summary>
        public static Type AssertIsAssignable<T>(string paramName, Type valType)
        {
            return AssertIsAssignable<T>(paramName
                    , valType
                    , string.Format("Type '{0}' of parameter '{1}' is not assignable to target type '{2}'"
                        , valType == null ? "<undefined>" : valType.AssemblyQualifiedName
                        , paramName
                        , typeof(T).AssemblyQualifiedName)
                );
        }

        /// <summary>
        /// Throws a <see cref="ArgumentOutOfRangeException"/> if an object of type <paramref name="valType"/> is not
        /// assignable to type <typeparam name="T"></typeparam>.
        /// </summary>
        public static Type AssertIsAssignable<T>(string paramName, Type valType, string messageFormat, params object[] args)
        {
            if (valType == null)
            {
                throw new ArgumentNullException("valType");
            }

            if (!typeof(T).IsAssignableFrom(valType))
            {
                throw new ArgumentOutOfRangeException(paramName, valType, string.Format(messageFormat, args));
            }
            return valType;
        }

        /// <summary>
        /// An anonymous action delegate with no arguments and no return value.
        /// </summary>
        /// <seealso cref="Guard"/>
        public delegate void Action();

        /// <summary>
        /// Ensures any exception thrown by the given <paramref name="action"/> is wrapped with an
        /// <see cref="ConfigurationException"/>. 
        /// </summary>
        /// <remarks>
        /// If <paramref name="action"/> already throws a ConfigurationException, it will not be wrapped.
        /// </remarks>
        /// <param name="action">the action to execute</param>
        /// <param name="messageFormat">the message to be set on the thrown <see cref="ConfigurationException"/></param>
        /// <param name="args">args to be passed to <see cref="string.Format(string,object[])"/> to format the message</param>
        public static void Guard(Action action, string messageFormat, params object[] args)
        {
            Guard<int>(delegate
                              {
                                  action();
                                  return 0;
                              }
                              , messageFormat, args);
        }

        /// <summary>
        /// An anonymous action delegate with no arguments and no return value.
        /// </summary>
        /// <seealso cref="Guard{T}"/>
        public delegate T Function<T>();

        /// <summary>
        /// Ensures any exception thrown by the given <paramref name="function"/> is wrapped with an
        /// <see cref="ConfigurationException"/>. 
        /// </summary>
        /// <remarks>
        /// If <paramref name="function"/> already throws a ConfigurationException, it will not be wrapped.
        /// </remarks>
        /// <param name="function">the action to execute</param>
        /// <param name="messageFormat">the message to be set on the thrown <see cref="ConfigurationException"/></param>
        /// <param name="args">args to be passed to <see cref="string.Format(string,object[])"/> to format the message</param>
        public static T Guard<T>(Function<T> function, string messageFormat, params object[] args)
        {
            try
            {
                return function();
            }
            catch (ConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConfigurationException(string.Format(messageFormat, args), ex);
            }
        }
    }
}