﻿#region License

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

using Common.Logging.NLog;

/// <summary>
/// <para>This assembly contains the implementations to plug NLog 2.0 into Common.Logging.</para>
/// <para>For an example on how to configure
/// <list type="bullet">
/// <item>Common.Logging to render its output to Entlib, see <see cref="NLogLoggerFactoryAdapter"/>.</item>
/// <item>NLog to render its output to Common.Logging use <see cref="CommonLoggingTarget"/>.</item>
/// </list>
/// </para>
/// <para>Note, that you cannot use NLog in medium trust environments unless you use an unsigned build</para>
/// </summary>
internal static class AssemblyDoc
{
    // serves as assembly summary for NDoc3 (http://ndoc3.sourceforge.net)
}
