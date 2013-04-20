using Common.Logging;
using Common.Logging.Simple;
using System.Runtime.CompilerServices;


#region Common.Logging.Configuration
// Don't perform any forwarding for these as they have changed to use Common.Logging.Configuration.NameValueCollection
#endregion

#region Common.Logging.Simple
[assembly: TypeForwardedTo(typeof(AbstractSimpleLogger))]

// Don't perform any forwarding for these as this has changed to use Common.Logging.Configuration.NameValueCollection
// [assembly: TypeForwardedTo(typeof(AbstractSimpleLoggerFactoryAdapter))]

[assembly: TypeForwardedTo(typeof(CapturingLogger))]
[assembly: TypeForwardedTo(typeof(CapturingLoggerEvent))]
// Perform forwarding event if the signature has changed to allow the old xml configurations to work
[assembly: TypeForwardedTo(typeof(CapturingLoggerFactoryAdapter))]


[assembly: TypeForwardedTo(typeof(NoOpLogger))]
// Perform forwarding event if the signature has changed to allow the old xml configurations to work
[assembly: TypeForwardedTo(typeof(NoOpLoggerFactoryAdapter))]
#endregion

#region Common.Logging
[assembly: TypeForwardedTo(typeof(ConfigurationException))]
[assembly: TypeForwardedTo(typeof(FormatMessageHandler))]
[assembly: TypeForwardedTo(typeof(IConfigurationReader))]
[assembly: TypeForwardedTo(typeof(ILog))]
[assembly: TypeForwardedTo(typeof(ILoggerFactoryAdapter))]
[assembly: TypeForwardedTo(typeof(LogLevel))]
[assembly: TypeForwardedTo(typeof(LogManager))]
#endregion




