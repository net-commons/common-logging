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

using Common.Logging.EntLib;
using Common.Logging.Simple;

/// <summary>
/// <para>
/// This assembly contains the <see cref="EntLibLoggerFactoryAdapter"/> adapter 
/// to plug Microsoft Enterprise Logging Library 4.1. into the Common.Logging infrastructure.
/// </para>
/// <para>For an example on how to configure
/// <ul>
/// <li>Common.Logging to render its output to Entlib, see <see cref="EntLibLoggerFactoryAdapter"/>.</li>
/// <li>Entlib to render its output to Common.Logging use <see cref="CommonLoggingTraceListener"/>.</li>
/// </ul>
/// </para>
/// </summary>
internal static class AssemblyDoc
{
    // serves as assembly summary for NDoc3 (http://ndoc3.sourceforge.net)
}
