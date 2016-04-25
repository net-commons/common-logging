#region License

/*
 * Copyright © 2002-2013 the original author or authors.
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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Globalization;

namespace Common.Logging.Simple
{
    /// <summary>
    /// </summary>
    internal static class ExceptionFormatter
    {
        // constants
        private const String STANDARD_DELIMETER =
            "================================================================================\r\n";
        private const String INNERMOST_DELIMETER =
            "=======================================================(inner most exception)===\r\n";

        internal static String Format(Exception exception)
        {
            return Format(exception, CultureInfo.InvariantCulture);
        }

        internal static String Format(Exception exception, IFormatProvider formatProvider)
        {
            if (exception == null)
                return null;

            // push all inner exceptions onto stack
            var exceptionStack = new Stack<Exception>();
            var currentException = exception;
            while (currentException != null)
            {
                exceptionStack.Push(currentException);
                currentException = currentException.InnerException;
            }

            // go through inner exceptions in reverse order
            var sb = new StringBuilder();
            for (Int32 i = 1; exceptionStack.Count > 0; i++)
            {
                currentException = exceptionStack.Pop();
                FormatSingleException(formatProvider, sb, currentException, i);
            }

            // that's it; return result
            return sb.ToString();
        }

        private static void FormatSingleException(IFormatProvider formatProvider, StringBuilder sb, Exception exception,
            Int32 exceptionIndex)
        {
            OutputHeader(formatProvider, sb, exception, exceptionIndex);
            OutputDetails(formatProvider, sb, exception);
            OutputMessage(formatProvider, sb, exception);
            OutputProperties(formatProvider, sb, exception);
            OutputData(formatProvider, sb, exception);
            OutputStackTrace(formatProvider, sb, exception);
            sb.Append(STANDARD_DELIMETER);
        }

        private static void OutputHeader(IFormatProvider formatProvider, StringBuilder sb, Exception exception,
            Int32 exceptionIndex)
        {
            // output header:
            //
            //	=======================================================(inner most exception)===
            //	 (index) exception-type-name
            //  ================================================================================
            //
            sb.Append(exceptionIndex == 1 ? INNERMOST_DELIMETER : STANDARD_DELIMETER);
            sb.AppendFormat(formatProvider, " ({0}) {1}\r\n",
                exceptionIndex, exception.GetType().FullName);
            sb.Append(STANDARD_DELIMETER);
        }

        private static void OutputDetails(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
#if PORTABLE && !WinRT && !DOTNETCORE
            sb.AppendFormat(formatProvider, "Thread ID : {0}\r\n", Thread.CurrentThread.ManagedThreadId);
#endif

#if !PORTABLE
            // output exception details:
            //
            //	Method        :  set_Attributes
            //	Type          :  System.IO.FileSystemInfo
            //	Assembly      :  mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
            //	Assembly Path :  C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll
            //	Source        :  mscorlib
            //	Thread        :  123 'TestRunnerThread'
            //  Helplink      :  <unavailable>
            //
            String assemblyName, assemblyModuleName, typeName, methodName;
            String source, helplink;

            SafeGetTargetSiteInfo(exception, out assemblyName, out assemblyModuleName, out typeName, out methodName);
            SafeGetSourceAndHelplink(exception, out source, out helplink);

            sb.AppendFormat(formatProvider,
                "Method        :  {0}\r\n" +
                "Type          :  {1}\r\n" +
                "Assembly      :  {2}\r\n" +
                "Assembly Path :  {3}\r\n" +
                "Source        :  {4}\r\n" +
                "Thread        :  {5} '{6}'\r\n" +
                "Helplink      :  {7}\r\n",
                methodName, typeName, assemblyName, assemblyModuleName,
                source,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name,
                helplink);
#endif
        }

        private static void OutputMessage(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
            // output exception message:
            //
            //	Message:
            //	"..."
            //
            sb.AppendFormat(formatProvider, "\r\nMessage:\r\n\"{0}\"\r\n",
                exception.Message);
        }

        private static void OutputProperties(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
            // output exception properties:
            //
            //	Properties:
            //	  ArgumentException.ParamName = "text"
            //
#if WinRT
            var properties = exception.GetType().GetProperties();

#else
            var properties = exception.GetType().GetProperties(BindingFlags.FlattenHierarchy |
                BindingFlags.Instance | BindingFlags.Public);
#endif
            Boolean first = true;
            foreach (PropertyInfo property in properties)
            {
                if (property.DeclaringType == typeof(Exception))
                    continue;
                if (property.Name == "Message")
                    continue;

                if (first)
                {
                    first = false;
                    sb.Append("\r\nProperties:\r\n");
                }

                Object propertyValue = "<unavailable>";
                if (property.CanRead)
                {
                    //if property is indexer, has no idea how to output that,
                    //and, also, without second param GetValue throw error in this case
                    //so, simple skip this
                    if (property.GetIndexParameters().Length <= 0)
                        propertyValue = property.GetValue(exception, null);
                }

                var enumerableValue = propertyValue as IEnumerable;
#if WinRT || DOTNETCORE
                var propertyTypeName = property.PropertyType.Name;
#else
                var propertyTypeName = property.ReflectedType.Name;
#endif
                if (enumerableValue == null || propertyValue is String)
                {
                    sb.AppendFormat(formatProvider, "  {0}.{1} = \"{2}\"\r\n",
                        propertyTypeName, property.Name, propertyValue);
                }
                else
                {
                    sb.AppendFormat(formatProvider, "  {0}.{1} = {{\r\n",
                        propertyTypeName, property.Name);

                    foreach (var item in enumerableValue)
                        sb.AppendFormat("    \"{0}\",\r\n", item != null ? item.ToString() : "<null>");

                    sb.Append("  }\r\n");
                }
            }
        }

        private static void OutputData(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
            // output exception properties:
            //
            //	Data:
            //	  Name = "value"
            //
            if (exception.Data.Count > 0)
            {
                sb.Append("\r\nData:\r\n");
                foreach (DictionaryEntry entry in exception.Data)
                {
                    sb.AppendFormat(formatProvider,
                        "{0} = \"{1}\"\r\n",
                        entry.Key, entry.Value);
                }
            }
        }

        private static void OutputStackTrace(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
            // output stack trace:
            //
            //	Stack Trace:
            //	  at System.IO.FileSystemInfo.set_Attributes(FileAttributes value)
            //    at Common.Logging.LogStoreWriter._SetupRootFolder() 
            //    at Common.Logging.LogStoreWriter..ctor(String rootPath, Int32 maxStoreSize, Int32 minBacklogs) 
            //
            sb.AppendFormat(formatProvider, "\r\nStack Trace:\r\n{0}\r\n",
                exception.StackTrace);
        }

#if !PORTABLE
        private static void SafeGetTargetSiteInfo(Exception exception, out String assemblyName, out String assemblyModulePath,
           out String typeName, out String methodName)
        {
            assemblyName = "<unavailable>";
            assemblyModulePath = "<unavailable>";
            typeName = "<unavailable>";
            methodName = "<unavailable>";

            MethodBase targetSite = exception.TargetSite;

            if (targetSite != null)
            {
                methodName = targetSite.Name;
                Type type = targetSite.ReflectedType;

                typeName = type.FullName;
                Assembly assembly = type.Assembly;

                assemblyName = assembly.FullName;
                Module assemblyModule = assembly.ManifestModule;

                assemblyModulePath = assemblyModule.FullyQualifiedName;
            }
        }

        private static void SafeGetSourceAndHelplink(Exception exception, out String source, out String helplink)
        {
            source = exception.Source;
            helplink = exception.HelpLink;
        }
#endif

    }
}