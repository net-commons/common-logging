// Copyright © Anton Paar GmbH, 2004-2013

using System;
using System.Collections;
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
        private const String DELIMITER_LINE =
            "================================================================================\r\n";
        private const String INNERMOST_DELIMITER_LINE =
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
            var exceptionStack = new Stack();
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
                currentException = (Exception)exceptionStack.Pop();
                _FormatSingleException(formatProvider, sb, currentException, i);
            }

            // that's it; return result
            return sb.ToString();
        }

        private static void _FormatSingleException(IFormatProvider formatProvider, StringBuilder sb, Exception exception,
            Int32 exceptionIndex)
        {
            _OutputHeader(formatProvider, sb, exception, exceptionIndex);
            _OutputDetails(formatProvider, sb, exception);
            _OutputMessage(formatProvider, sb, exception);
            _OutputProperties(formatProvider, sb, exception);
            _OutputData(formatProvider, sb, exception);
            _OutputStackTrace(formatProvider, sb, exception);
            sb.Append(DELIMITER_LINE);
        }

        private static void _OutputHeader(IFormatProvider formatProvider, StringBuilder sb, Exception exception,
            Int32 exceptionIndex)
        {
            // output header:
            //
            //	=======================================================(inner most exception)===
            //	 (index) exception-type-name
            //  ================================================================================
            //
            sb.Append(exceptionIndex == 1 ? INNERMOST_DELIMITER_LINE : DELIMITER_LINE);
            sb.AppendFormat(formatProvider, " ({0}) {1}\r\n",
                exceptionIndex, exception.GetType().FullName);
            sb.Append(DELIMITER_LINE);
        }

        private static void _OutputDetails(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
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

            _SafeGetTargetSiteInfo(exception, out assemblyName, out assemblyModuleName, out typeName, out methodName);
            _SafeGetSourceAndHelplink(exception, out source, out helplink);

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
        }

        private static void _OutputMessage(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
            // output exception message:
            //
            //	Message:
            //	"..."
            //
            sb.AppendFormat(formatProvider, "\r\nMessage:\r\n\"{0}\"\r\n",
                exception.Message);
        }

        private static void _OutputProperties(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
        {
            // output exception properties:
            //
            //	Properties:
            //	  ArgumentException.ParamName = "text"
            //
            var properties = exception.GetType().GetProperties(BindingFlags.FlattenHierarchy |
                BindingFlags.Instance | BindingFlags.Public);

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
                    propertyValue = property.GetValue(exception, null);

                var enumerableValue = propertyValue as IEnumerable;

                if (enumerableValue == null || propertyValue is String)
                {
                    sb.AppendFormat(formatProvider, "  {0}.{1} = \"{2}\"\r\n",
                        property.ReflectedType.Name, property.Name, propertyValue);
                }
                else
                {
                    sb.AppendFormat(formatProvider, "  {0}.{1} = {{\r\n",
                        property.ReflectedType.Name, property.Name);

                    foreach (var item in enumerableValue)
                        sb.AppendFormat("    \"{0}\",\r\n", item != null ? item.ToString() : "<null>");

                    sb.Append("  }\r\n");
                }
            }
        }

        private static void _OutputData(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
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

        private static void _OutputStackTrace(IFormatProvider formatProvider, StringBuilder sb, Exception exception)
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

        private static void _SafeGetTargetSiteInfo(Exception exception, out String assemblyName, out String assemblyModulePath,
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

        private static void _SafeGetSourceAndHelplink(Exception exception, out String source, out String helplink)
        {
            source = exception.Source;
            helplink = exception.HelpLink;
        }
    }
}