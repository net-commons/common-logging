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

            //read any enable/disable logging types and react accordingly
            var isTraceEnabled = properties?["isTraceEnabled"];
            var isDebugEnabled = properties?["isDebugEnabled"];
            var isErrorEnabled = properties?["isErrorEnabled"];
            var isFatalEnabled = properties?["isFatalEnabled"];
            var isInfoEnabled = properties?["isInfoEnabled"];
            var isWarnEnabled = properties?["isWarnEnabled"];

            //create a collection of these to make it simpler to analyze whether any are actually specified
            var enablingDirectives = new List<string>
            {
                isTraceEnabled,
                isDebugEnabled,
                isErrorEnabled,
                isFatalEnabled,
                isInfoEnabled,
                isWarnEnabled
            };

            //if any enabling directives are set ...
            if (enablingDirectives.Any(setting => setting != null))
            {
                //...create a config instance with all properties set to specified values, defaulting to FALSE
                //  for any values unspecified in properties
                var config = new ETWLoggerConfiguration
                {
                    IsTraceEnabled = IsSettingEnabled(isTraceEnabled),
                    IsDebugEnabled = IsSettingEnabled(isDebugEnabled),
                    IsErrorEnabled = IsSettingEnabled(isErrorEnabled),
                    IsFatalEnabled = IsSettingEnabled(isFatalEnabled),
                    IsInfoEnabled = IsSettingEnabled(isInfoEnabled),
                    IsWarnEnabled = IsSettingEnabled(isWarnEnabled)
                };

                Configuration = config;

            }
            else
            {
                //...else just create the configuration object with all defaults
                Configuration = new ETWLoggerConfiguration();
            }
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