using System.Reflection;
using System.Security;

[assembly: AssemblyProduct("Common Logging Framework Application Insights Adapter")]

// Application Insights does not allow Partially Trust Callers 
// it cannot set [assembly: AllowPartiallyTrustedCallers()] attribute and therefore does not use
// CommonAssemblyInfo.cs

[assembly: AssemblyCompanyAttribute("http://netcommon.sourceforge.net/")]
[assembly: AssemblyCopyrightAttribute("Copyright 2006-2015 the Common Infrastructure Libraries Team.")]
[assembly: AssemblyTrademarkAttribute("Apache License, Version 2.0")]
[assembly: AssemblyCultureAttribute("")]
[assembly: AssemblyVersionAttribute("3.1.0")]
[assembly: AssemblyDelaySignAttribute(false)]
