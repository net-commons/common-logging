using System.Diagnostics;

namespace Common.TestUtil
{
    public class TraceEventArgs
    {
        public string Source;
        public TraceEventType EventType;
        public int Id;
        public string Format;
        public object[] Args;

        public TraceEventArgs(string source, TraceEventType eventType, int id, string format, object[] args)
        {
            Source = source;
            EventType = eventType;
            Id = id;
            Format = format;
            Args = args;
        }

        public string FormattedMessage
        {
            get
            {
                if (Args ==  null)
                {
                    return Format;
                }
                return string.Format(Format, Args);
            }
        }
    }
}