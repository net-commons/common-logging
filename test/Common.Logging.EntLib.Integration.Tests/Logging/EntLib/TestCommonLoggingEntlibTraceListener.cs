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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common.TestUtil;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Common.Logging.EntLib
{
    /// <summary>
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class TestCommonLoggingEntlibTraceListener : CommonLoggingEntlibTraceListener
    {
        public static readonly List<TraceEventArgs> Events = new List<TraceEventArgs>();

        private bool logEnabled = false;

        private class CaptureContext : IDisposable
        {
            private readonly TestCommonLoggingEntlibTraceListener _owner;

            public CaptureContext(TestCommonLoggingEntlibTraceListener owner)
            {
                owner.logEnabled = true;
                _owner = owner;
            }

            public void Dispose()
            {
                _owner.logEnabled = false;
            }
        }

        public static TestCommonLoggingEntlibTraceListener Instance;

        public TestCommonLoggingEntlibTraceListener(CommonLoggingEntlibTraceListenerData data, ILogFormatter logFormatter)
            : base(data, logFormatter)
        {
            if (Instance != null)
            {
                throw new NotSupportedException(this.GetType().FullName + " supports only one instance");
            }
            Instance = this;
        }

        public IDisposable Capture()
        {
            return new CaptureContext(this);
        }

        protected override void Log(TraceEventType eventType, string source, int id, string format, params object[] args)
        {
            Events.Add(new TraceEventArgs(null, source, eventType, null, id, null, null, null, null));
            if (logEnabled)
            {
                base.Log(eventType, source, id, format, args);
            }
        }

        public override void Write(string message)
        {
            // ensure we don't get called accidentially
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            // ensure we don't get called accidentially
            throw new NotImplementedException();
        }
    }
}