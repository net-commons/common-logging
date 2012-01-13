Common.Logging 2.0 (April 26 2009)
---------------------------------------------------------
http://netcommon.sf.net/


1. INTRODUCTION

Provides a simple logging abstraction to switch between different logging implementations.
There is current support for log4net (1.2.10 and 1.2.9), NLog, Enterprise Library logging 3.1
and Enterprise Library logging 4.1.
Additionally Common.Logging comes with a set of base classes making integration of any log
system a breeze.

2. KNOWN ISSUES

No known issues

3. RELEASE INFO

Release contents:

* "bin" contains the Common.Logging distribution dll files
* "doc" contains reference documentation.
* "shared" contains shared nant build scripts
* "modules" contains the sourcecode for Common.Logging.XXXX modules
* "redist" contains redistributable assemblies like log4net for your convenience

The Common Infrastructure Libraries for .NET are released under the terms of the Apache Software License (see license.txt).


4. BUILDING

For building Common.Logging library, you need to Enterprise Library 3.1 and Enterprise Library 4.1 installed.

After installation of the Enterprise Library packages from Microsoft but before you build you must copy the necessary binary references from their installed location(s) into the required location within the source tree so that the required references are properly resolved during the build:

Enterprise Library 3.1
Files required from c:\program files\Microsoft Enterprise Library 3.1 - May 2007\bin\ (assumes default installation location of Enterprise Library 3.1):
  * Microsoft.Practices.EnterpriseLibrary.Common.dll
  * Microsoft.Practices.EnterpriseLibrary.Logging.dll
  * Microsoft.Practices.ObjectBuilder.dll

Copy the above 3 files into C:\netcommon\lib\Entlib\net\2.0\

Enterprise Library 4.1
Files required from c:\program files\Microsoft Enterprise Library 4.1 - October 2008\bin\ (assumes default installation location of Enterprise Library 4.1):
  * Microsoft.Practices.EnterpriseLibrary.Common.dll
  * Microsoft.Practices.EnterpriseLibrary.Logging.dll
  * Microsoft.Practices.ObjectBuilder2.dll

Copy the above 3 files into C:\netcommon\lib\Entlib\net\3.5\

From the commandline execute

  c:\netcommon>nant build-package


5. CONFIGURING

in your app.config or web.config add the lines

<?xml version="1.0" encoding="utf-8" ?>
<configuration>

  <configSections>
    <sectionGroup name="common">
      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
    </sectionGroup>
  </configSections>

  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Simple.TraceLoggerFactoryAdapter, Common.Logging">
        <arg key="level" value="ALL" />
      </factoryAdapter>
    </logging>
  </common>

...

</configuration>


see module documentations for available factoryAdapters and their configuration values

