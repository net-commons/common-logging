using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging.Configuration;
using Common.Logging.Factory;

namespace Common.Logging.ETW
{
    public class ETWLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {

        //static (shared) dictionary of all EventSource subclasses used by *all* instances of this adapter
        // NOTE: this field is static so that the adapter may enforce single registration of each EventSource subclass type
        //          across *all* instances of this adapter in a single AppDomain.  Duplicate ctor invocations of any single EventSource
        //          subclass will otherwise result in errors (possibly accompanied by missed log entries) in the ETW subsystem in the OS)
        //       see https://github.com/net-commons/common-logging/issues/125 for more details.
        private static readonly Dictionary<Type, ICommonLoggingEventSource> EventSourceRegistry = new Dictionary<Type, ICommonLoggingEventSource>();

        //the type of event source used by this adapter; acts as a KEY to the shared static dictionary of registrations
        private Type _eventSourceType;

        //controls whether to throw on attempts to duplicate any event source type registrations
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
            _permitDuplicateEventSourceRegistration = ArgUtils.TryParse(false, ArgUtils.GetValue(properties, "permitDuplicateEventSourceRegistration"));
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
                    //...add it
                    EventSourceRegistry.Add(eventSourceType, candidate);
                }
                else
                {
                    //if the incoming instance is _not_ the same instance as that already registered...
                    if (EventSourceRegistry[eventSourceType] != candidate)
                    {
                        //...react accordingly to the attempted duplicate registration
                        ThrowIfDuplicateEventSourceTypeRegistrationNotPermitted(eventSourceType);
                    }

                    //if we get this far, replace the existing instance with the new instance
                    EventSourceRegistry[eventSourceType] = candidate;
                }

                //set the type designator so we can keep track of the correct EventSource Type for this adapter
                _eventSourceType = eventSourceType;
            }
        }

        private void ThrowIfDuplicateEventSourceTypeRegistrationNotPermitted(Type eventSourceType)
        {
            if (!_permitDuplicateEventSourceRegistration)
            {
                var message = new StringBuilder();
                message.AppendLine("Error attempting to register an instance of type " + eventSourceType.FullName + " as an EventSource on the ETWLoggerFactoryAdapters.");
                message.AppendLine("Attempting to register an EventSource-derived subclass with the adapter that has already been registered is not supported.");
                message.AppendLine("By default, Common.Logging will not permit more than a single registration of each EventSource subclass across all ETWLoggerFactoryAdapters in a single AppDomain.");
                message.AppendLine("For more details as to the reasoning for this constraint, see https://github.com/net-commons/common-logging/issues/125");
                message.AppendLine("To opt out of Common.Logging\'s enforcement of this constraint (at your own risk), set <arg key=\'permitDuplicateEventSourceRegistration\' value=\'true\'/>  in your config file.");


                throw new InvalidOperationException(message.ToString());
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