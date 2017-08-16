using System.Reflection;
using System.Security;

[assembly: AssemblyProduct("Common Logging Framework NLog 4.4.12 Adapter")]
[assembly: SecurityTransparent]

#if NET_4_0
[assembly: SecurityRules(SecurityRuleSet.Level1)]
#endif