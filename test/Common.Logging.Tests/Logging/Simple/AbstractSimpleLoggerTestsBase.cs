#region License

/*
 * Copyright © 2002-2007 the original author or authors.
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

using Common.Logging.Configuration;


namespace Common.Logging.Simple
{
    /// <summary>
    /// Base class that exercises the basic api of the simple loggers. 
    /// To simplify testing derived classes you should derive your fixtures from this base fixture
    /// </summary>
    /// <author>Mark Pollack</author>
    public abstract class AbstractSimpleLoggerTestsBase : ILogTestsBase
    {
        private static int count;

        protected static NameValueCollection CreateProperties()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            //            if ((count % 2) == 0)
            {
                properties["dateTimeFormat"] = "yyyy/MM/dd HH:mm:ss:fff";
            }
            count++;
            return properties;
        }
    }
}
