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

namespace Common.Logging
{
//    /// <summary>
//    /// The type of a callback method used to obtain a message to be logged. 
//    /// </summary>
//    /// <remarks>
//    /// Each <see cref="ILog"/> log method offers to pass in such a callback instead of the actual message.
//    /// Using this methods has the advantage to defer possibly expensive message formatting until the message gets
//    /// actually logged. If the message is not logged at all (e.g. due to <see cref="LogLevel"/> settings), 
//    /// you won't have to pay the peformance penalty of creating the message.
//    /// </remarks>
//    /// <example>
//    /// The example below demonstrates using callback style for creating the message:
//    /// <code>
//    /// Log.Debug( m=&gt;m(&quot;result is {0}&quot;, random.NextDouble()) );
//    /// Log.Debug(delegate(m) { m(&quot;result is {0}&quot;, random.NextDouble()); });
//    /// </code>
//    /// </example>
//    /// <seealso cref="ILog"/>
//    /// <author>Erich Eichinger</author>
//    public delegate void FormatMessageCallback(FormatMessageHandler fmcb);
}