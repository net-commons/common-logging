#region License

/*
 * Copyright 2002-2009 the original author or authors.
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

using Common.Logging;
using Common.Logging.Factory;
using Common.Logging.Simple;

/// <summary>
    /// <h1>Overview</h1>
    /// <para>
    /// There are a variety of logging implementations for .NET currently in use, log4net, Enterprise 
    /// Library Logging, NLog, to name the most popular. The downside of having differerent implementation 
    /// is that they do not share a common interface and therefore impose a particular logging 
    /// implementation on the users of your library. To solve this dependency problem the Common.Logging 
    /// library introduces a simple abstraction to allow you to select a specific logging implementation at 
    /// runtime.
    /// </para>
    /// <para>
    /// The library is based on work done by the developers of IBatis.NET and it's usage is inspired by 
    /// log4net. Many thanks to the developers of those projects!
    /// </para>
    /// <h1>Usage</h1>
    /// <para>
    /// The core logging library Common.Logging provides the base logging <see cref="ILog"/> interface as 
    /// well as the global <see cref="LogManager"/> that you use to instrument your code:
    /// </para>
    /// <code lang="C#">
    /// ILog log = LogManager.GetLogger(this.GetType());  
    /// 
    /// log.DebugFormat(&quot;Hi {0}&quot;, &quot;dude&quot;);
    /// </code>
    /// <para>
    /// To output the information logged, you need to tell Common.Logging, what underlying logging system 
    /// to use. Common.Logging already includes simple console and trace based logger implementations 
    /// usable out of the box. Adding the following configuration snippet to your app.config causes 
    /// Common.Logging to output all information to the console:
    /// </para>
    /// <code lang="XML">
    /// &lt;configuration&gt; 
    ///     &lt;configSections&gt; 
    ///       &lt;sectionGroup name=&quot;common&quot;&gt; 
    ///         &lt;section name=&quot;logging&quot; type=&quot;Common.Logging.ConfigurationSectionHandler, Common.Logging&quot; /&gt; 
    ///       &lt;/sectionGroup&gt;  
    ///     &lt;/configSections&gt; 
    ///      
    ///     &lt;common&gt; 
    ///       &lt;logging&gt; 
    ///         &lt;factoryAdapter type=&quot;Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging&quot;&gt; 
    ///           &lt;arg key=&quot;level&quot; value=&quot;DEBUG&quot; /&gt; 
    ///         &lt;/factoryAdapter&gt; 
    ///       &lt;/logging&gt; 
    ///     &lt;/common&gt; 
    /// &lt;/configuration&gt; 
    /// </code>
    /// <h1>Customizing</h1>
    /// <para>
    /// In the case you want to integrate your own logging system that is not supported by Common.Logging yet, it is easily 
    /// possible to implement your own plugin by implementing <see cref="ILoggerFactoryAdapter" />.
    /// For convenience there is a base <see cref="AbstractCachingLoggerFactoryAdapter"/> implementation available that usually 
    /// makes implementing your own adapter a breeze.
    /// </para>
    /// <h1>&lt;system.diagnostics&gt; Integration</h1>
    /// <para>
    /// If your code already uses the .NET framework's built-in <a href="http://msdn.microsoft.com/library/system.diagnostics.trace.aspx">System.Diagnostics.Trace</a>
    /// system, you can use <see cref="CommonLoggingTraceListener" /> to redirect all trace output to the 
    /// Common.Logging infrastructure.
    /// </para>
    /// </summary>
    [CoverageExclude]
    internal static class NamespaceDoc
    {
        // serves as namespace summary for NDoc3 (http://ndoc3.sourceforge.net)
        // - used to generate ./doc/api/introduction.html
    }
