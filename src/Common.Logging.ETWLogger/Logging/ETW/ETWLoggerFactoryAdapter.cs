using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging.Configuration;
using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public class ETWLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        public ETWLoggerConfiguration Configuration { get; set; }

        public ICommonLoggingEventSource ETWEventSource { get; set; }

        public ETWLoggerFactoryAdapter()
            : this(null)
        { }

        public ETWLoggerFactoryAdapter(NameValueCollection properties)
            : base(true)
        {
            //assign an instance of a custom ICommonLoggingEventSource if specified, else use default type
            var commonLoggingEventSourceTypeDescriptor = properties?["commonLoggingEventSourceType"];

            if (commonLoggingEventSourceTypeDescriptor != null)
            {
                var type = Type.GetType(commonLoggingEventSourceTypeDescriptor);

                if (type != null)
                {
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
                        throw new ConfigurationException("Error in commonLoggingEventSourceType property.", exception);
                    }
                }
            }
            else
            {
                ETWEventSource = new CommonLoggingEventSource();
            }

            //as a default, we log NOTHING
            LogLevel logLevel = LogLevel.Off;

            //read the value from the config
            var levelSetting = properties?["level"];

            //if we have a value, react accordingly
            if (!string.IsNullOrWhiteSpace(levelSetting))
            {
                switch (levelSetting.ToUpper())
                {
                    case "TRACE":
                        logLevel = LogLevel.Trace;
                        break;

                    case "DEBUG":
                        logLevel = LogLevel.Debug | LogLevel.Trace;
                        break;

                    case "INFO":
                        logLevel = LogLevel.Info | LogLevel.Debug | LogLevel.Trace;
                        break;

                    case "WARN":
                        logLevel = LogLevel.Warn | LogLevel.Info | LogLevel.Debug | LogLevel.Trace;
                        break;

                    case "ERROR":
                        logLevel = LogLevel.Error | LogLevel.Warn | LogLevel.Info | LogLevel.Debug | LogLevel.Trace;
                        break;

                    case "FATAL":
                    case "ALL":
                        logLevel = LogLevel.Fatal | LogLevel.Error | LogLevel.Warn | LogLevel.Info | LogLevel.Debug | LogLevel.Trace;
                        break;

                    //if we don't get a valid value, throw
                    default:
                        throw new ConfigurationException("Invalid value for 'level' argument in configuration.");
                }

            }

            Configuration = new ETWLoggerConfiguration() { LogLevel = logLevel };
        }

        private bool IsSettingEnabled(string setting)
        {
            if (null == setting)
                return false;

            return setting.ToUpper() == "TRUE" || setting == "1" || setting == "YES";
        }


        protected override ILog CreateLogger(string name)
        {
            //TODO: determine whether we should be bothering to respect the 'name' arg here
            //  (probably NOT b/c ETW doesn't actually rely upon diff. instances of loggers for diff. types)
            return new ETWLogger(ETWEventSource, Configuration);
        }
    }
}