using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging.Configuration;
using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public class ETWLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {


        private static readonly Dictionary<Type, ICommonLoggingEventSource> EventSourceRegistry = new Dictionary<Type, ICommonLoggingEventSource>();

        private Type _eventSourceType;
        private bool _permitDuplicateEventSourceRegistration;

        public LogLevel LogLevel { get; set; }

        public ICommonLoggingEventSource EventSource
        {
            get
            {
                return EventSourceRegistry[_eventSourceType];
            }
            set
            {
                RecordEventSource(value);
            }
        }

        public ETWLoggerFactoryAdapter()
            : this(null)
        { }

        public ETWLoggerFactoryAdapter(NameValueCollection properties)
            : base(true)
        {
            CheckPermitDuplicateEventSourceRegistration(properties);
            ConfigureEventSource(properties);
            ConfigureLogLevel(properties);
        }

        private void CheckPermitDuplicateEventSourceRegistration(NameValueCollection properties)
        {
            //TODO: parse properties to see if 'ignore duplicate event source' param is set

            //for now, *DON'T* throw
            _permitDuplicateEventSourceRegistration = false;
        }

        private void ConfigureLogLevel(NameValueCollection properties)
        {
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

        private void ConfigureEventSource(NameValueCollection properties)
        {
            //assign an instance of a custom ICommonLoggingEventSource if specified, else use default type
            var commonLoggingEventSourceTypeDescriptor = ArgUtils.GetValue(properties, "commonLoggingEventSourceType",
                string.Empty);

            if (!string.IsNullOrEmpty(commonLoggingEventSourceTypeDescriptor))
            {
                var eventSourceType = Type.GetType(commonLoggingEventSourceTypeDescriptor);

                if (null == eventSourceType)
                {
                    throw new ConfigurationException(
                        string.Format(
                            "Error in 'commonLoggingEventSourceType' arg.  Unable to determine TYPE information from value {0}",
                            commonLoggingEventSourceTypeDescriptor));
                }

                try
                {
                    var candidate = Activator.CreateInstance(eventSourceType) as ICommonLoggingEventSource;
                    RecordEventSource(candidate);
                }
                catch (Exception exception) //no matter the underlying exception type we want to report it as a Config Exception
                {
                    throw new ConfigurationException("Error in 'commonLoggingEventSourceType' arg.", exception);
                }
            }
            else
            {
                var defaultEventSourceType = typeof(CommonLoggingEventSource);

                if (!EventSourceRegistry.ContainsKey(defaultEventSourceType))
                {
                    EventSource = new CommonLoggingEventSource();
                }

                _eventSourceType = defaultEventSourceType;
            }
        }

        private void RecordEventSource(ICommonLoggingEventSource candidate)
        {

            //if we have a valid object
            if (candidate != null)
            {
                var eventSourceType = candidate.GetType();

                //if we don't already have an instance of this same type in the registry...
                if (!EventSourceRegistry.ContainsKey(eventSourceType))
                {
                    EventSourceRegistry.Add(eventSourceType, candidate);
                }
                else
                {
                    //process the condition where we have a duplicate
                    ThrowIfDuplicateEventSourceTypeRegistrationNotPermitted();

                    //if we get this far, replace the existing instance with the new instance
                    EventSourceRegistry[eventSourceType] = candidate;
                }

                //set the type designator so we can keep track of the correct EventSource Type for this adapter
                _eventSourceType = eventSourceType;
            }
        }

        private void ThrowIfDuplicateEventSourceTypeRegistrationNotPermitted()
        {
            if (!_permitDuplicateEventSourceRegistration)
            {
                //TODO: expand on the detail in this exception
                throw new InvalidOperationException();
            }
        }

        protected override ILog CreateLogger(string name)
        {
            //TODO: determine whether we should be bothering to respect the 'name' arg here
            //  (probably NOT b/c ETW doesn't actually rely upon diff. instances of loggers for diff. types)
            return new ETWLogger(EventSource, LogLevel);
        }
    }
}