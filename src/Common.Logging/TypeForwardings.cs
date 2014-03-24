using Common.Logging;
using Common.Logging.Simple;
using System.Runtime.CompilerServices;


#region Common.Logging.Configuration
// Don't perform any forwarding for these as they have changed to use Common.Logging.Configuration.NameValueCollection
#endregion

#region Common.Logging.Simple

// Don't perform any forwarding for these as this has changed to use Common.Logging.Configuration.NameValueCollection
// [assembly: TypeForwardedTo(typeof(AbstractSimpleLoggerFactoryAdapter))]

// Perform forwarding event if the signature has changed to allow the old xml configurations to work


// Perform forwarding event if the signature has changed to allow the old xml configurations to work
#endregion

#region Common.Logging
[assembly: TypeForwardedTo(typeof(ConfigurationException))]
[assembly: TypeForwardedTo(typeof(FormatMessageHandler))]
[assembly: TypeForwardedTo(typeof(IConfigurationReader))]
[assembly: TypeForwardedTo(typeof(ILog))]
[assembly: TypeForwardedTo(typeof(ILoggerFactoryAdapter))]
[assembly: TypeForwardedTo(typeof(LogLevel))]
#endregion




