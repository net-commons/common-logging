Common.Logging 2.1.0 (June 8 2012)
---------------------------------------------------------
http://github.com/net-commons/common-logging
http://netcommon.sf.net/


1. INTRODUCTION

Provides a simple logging abstraction to switch between different logging implementations.
There is current support for log4net (1.2.11, 1.2.10 and 1.2.9), NLog (1.0, 2.0), Enterprise Library logging 3.1,
Enterprise Library logging 4.1, and Enterprise Library 5.0

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

Clone the github repository found at https://github.com/net-commons/common-logging using your choice of connection protocol:
  * git://github.com/net-commons/common-logging.git
  * https://github.com/net-commons/common-logging.git
  * git@github.com:net-commons/common-logging.git

From the commandline execute

  c:\netcommon>build-release.cmd


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

