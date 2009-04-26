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

For building Common.Logging library, you need to have NAnt, Enterprise Library 3.1 and Enterprise Library 4.1 installed.
From the commandline execute

  c:\netcommon>nant build


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

