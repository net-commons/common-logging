using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

#if !PORTABLE40
using System.Security.Permissions;
#endif

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.586
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: CLSCompliantAttribute(true)]

#if PORTABLE40
[assembly: AssemblyConfigurationAttribute("portable40; release")]
[assembly: AssemblyInformationalVersionAttribute("2.0.0; portable40; release")]
#else
// Note - if we decide to skip SL4+WP support and only Support SL5 then we should be able to specify AllowPartiallyTrustedCallersAttribute
// even for Common.Logging.Core
[assembly: AllowPartiallyTrustedCallers()]
[assembly: AssemblyConfigurationAttribute("net-4.0.win32; dev")]
[assembly: AssemblyInformationalVersionAttribute("2.0.0; net-4.0.win32; dev")]
#endif

[assembly: AssemblyCompanyAttribute("http://netcommon.sourceforge.net/")]
[assembly: AssemblyCopyrightAttribute("Copyright 2006-2011 the Common Infrastructure Libraries Team.")]
[assembly: AssemblyTrademarkAttribute("Apache License, Version 2.0")]
[assembly: AssemblyCultureAttribute("")]
[assembly: AssemblyVersionAttribute("2.0.0")]
[assembly: AssemblyDelaySignAttribute(false)]

