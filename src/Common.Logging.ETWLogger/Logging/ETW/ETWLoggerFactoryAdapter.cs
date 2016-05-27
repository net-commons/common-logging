using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging.Configuration;
using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public class ETWLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        public LogLevel LogLevel { get; set; }

        public ICommonLoggingEventSource ETWEventSource { get; set; }

        public ETWLoggerFactoryAdapter()
            : this(null)
        { }

        public ETWLoggerFactoryAdapter(NameValueCollection properties)
            : base(true)
        {
            //assign an instance of a custom ICommonLoggingEventSource if specified, else use default type
            var commonLoggingEventSourceTypeDescriptor = ArgUtils.GetValue(properties, "commonLoggingEventSourceType", string.Empty);

            if (!string.IsNullOrEmpty(commonLoggingEventSourceTypeDescriptor))
            {
                var type = Type.GetType(commonLoggingEventSourceTypeDescriptor);

                if (null == type)
                {
                    throw new ConfigurationException(
                        $"Error in 'commonLoggingEventSourceType' arg.  Unable to determine TYPE information from value {commonLoggingEventSourceTypeDescriptor}");
                }

                try
                {
                    var candidate = Activator.CreateInstance(type) as ICommonLoggingEventSource;

                    if (candidate != null)
                    {
                        ETWEventSource = candidate;
                    }

                }
                catch (Exception exception) //no matter the underlying exception type we want to report it as a Config Exception
                {
                    throw new ConfigurationException("Error in 'commonLoggingEventSourceType' arg.", exception);
                }
            }
            else
            {
                ETWEventSource = new CommonLoggingEventSource();
            }

            //set the logging level; default to ALL
            var levelSetting = ArgUtils.TryParseEnum(LogLevel.All, ArgUtils.GetValue(properties, "level"));

            switch (levelSetting)
            {
                case LogLevel.Trace:
                case LogLevel.All:
                    LogLevel = LogLevel.Trace | LogLevel.Debug | LogLevel.Info | LogLevel.Warn | LogLevel.Error | LogLevel.Fatal;
                    break;

                case LogLevel.Debug:
                    LogLevel = LogLevel.Debug | LogLevel.Info | LogLevel.Warn | LogLevel.Error | LogLevel.Fatal;
                    break;

                case LogLevel.Info:
                    LogLevel = LogLevel.Info | LogLevel.Warn | LogLevel.Error | LogLevel.Fatal;
                    break;

                case LogLevel.Warn:
                    LogLevel = LogLevel.Warn | LogLevel.Error | LogLevel.Fatal;
                    break;

                case LogLevel.Error:
                    LogLevel = LogLevel.Error | LogLevel.Fatal;
                    break;

                case LogLevel.Fatal:
                    LogLevel = LogLevel.Fatal;
                    break;

                case LogLevel.Off:
                default:
                    break;
            }

        }

        protected override ILog CreateLogger(string name)
        {
            //TODO: determine whether we should be bothering to respect the 'name' arg here
            //  (probably NOT b/c ETW doesn't actually rely upon diff. instances of loggers for diff. types)
            return new ETWLogger(ETWEventSource, LogLevel);
        }
    }
}