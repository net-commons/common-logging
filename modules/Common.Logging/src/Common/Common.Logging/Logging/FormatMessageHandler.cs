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
    ///<summary>
    /// The type of method that is passed into a <see cref="FormatMessageCallback"/> and allows the callback
    /// method to "submit" it's message to the underlying output system.
    ///</summary>
    ///<param name="format">the format argument as in <see cref="string.Format(string,object[])"/></param>
    ///<param name="args">the argument list as in <see cref="string.Format(string,object[])"/></param>
    ///<seealso cref="ILog"/>
    ///<seealso cref="FormatMessageCallback"/>
    /// <author>Erich Eichinger</author>
    public delegate void FormatMessageHandler(string format, params object[] args);
}