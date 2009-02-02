using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.TestUtil
{
    public class CapturingTraceListener : TraceListener
    {
        public static readonly List<TraceEventArgs> Events = new List<TraceEventArgs>();

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            Events.Add(new TraceEventArgs(source, eventType, id, null, null));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            Events.Add(new TraceEventArgs(source, eventType, id, message, null));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            Events.Add(new TraceEventArgs(source, eventType, id, format, args));
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
