namespace Common.Logging.ETW
{
    public interface ICommonLoggingEventSource
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);

        void TraceException(string message, string exception);
        void DebugException(string message, string exception);
        void InfoException(string message, string exception);
        void WarnException(string message, string exception);
        void ErrorException(string message, string exception);
        void FatalException(string message, string exception);
    }
}