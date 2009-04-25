using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.TestUtil
{
    public class CapturingTraceListener : TraceListener
    {
        public static readonly List<TraceEventArgs> Events = new List<TraceEventArgs>();

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            CaptureEvent(new TraceEventArgs(eventCache, source, eventType, null, id, null, null, new object[] {data }, null));
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            CaptureEvent(new TraceEventArgs(eventCache, source, eventType, null, id, null, null, data, null));
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            CaptureEvent(new TraceEventArgs(eventCache, source, null, null, id, message, null, null, relatedActivityId));
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            CaptureEvent(new TraceEventArgs(eventCache, source, eventType, null, id, null, null, null, null));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            CaptureEvent(new TraceEventArgs(eventCache, source, eventType, null, id, message, null, null, null));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            CaptureEvent(new TraceEventArgs(eventCache, source, eventType, null, id, format, args, null, null));
        }

        public override void Write(object o)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, null, null, o, null, null, null));
        }

        public override void Write(string message)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, null, null, message, null, null, null));
        }

        public override void Write(object o, string category)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, category, null, o, null, null, null));
        }

        public override void Write(string message, string category)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, category, null, message, null, null, null));
        }

        public override void WriteLine(object o)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, null, null, o, null, null, null));
        }

        public override void WriteLine(object o, string category)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, category, null, o, null, null, null));
        }

        public override void WriteLine(string message)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, null, null, message, null, null, null));
        }

        public override void WriteLine(string message, string category)
        {
            CaptureEvent(new TraceEventArgs(null, null, null, category, null, message, null, null, null));
        }

        protected virtual void CaptureEvent(TraceEventArgs eventArgs)
        {
            Events.Add(eventArgs);
        }
    }
}
