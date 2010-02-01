using System;
using System.Diagnostics;
using System.Text;

namespace Common.TestUtil
{
    public class TraceEventArgs
    {
        public TraceEventCache EventCache;
        public string Source;
        public TraceEventType? EventType;
        public string Category;
        public int? Id;
        public object[] Data;
        public object Format;
        public object[] Args;
        public Guid? RelatedActivityId;

        public TraceEventArgs(TraceEventCache eventCache, string source, TraceEventType? eventType, string category, int? id, object message, object[] args, object[] data, Guid? relatedActivityId)
        {
            EventCache = eventCache;
            Source = source;
            EventType = eventType;
            Category = category;
            Id = id;
            Format = message;
            Args = args;
            Data = data;
            RelatedActivityId = relatedActivityId;
        }

        public string FormattedMessage
        {
            get
            {
                string msg = null;
                if (Format ==  null && Data != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach(object d in Data)
                    {
                        sb.Append(d);
                    }
                    msg = sb.ToString();
                }
                else
                {
                    msg = "" + Format;
                }
                if (Args != null)
                {
                    return string.Format(msg, Args);
                }
                return msg;
            }
        }
    }
}