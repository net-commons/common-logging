# Common.Logging .NET
## Introduction

Provides a simple logging abstraction to switch between different logging implementations.
There is current support for log4net, NLog, and Enterprise Library logging.

Additionally Common.Logging comes with a set of base classes making the integration of any log
system a breeze.

See also

* [Github](http://github.com/net-commons/common-logging)
* [Source Forge](http://netcommon.sf.net/)
* [Nuget](https://www.nuget.org/packages/Common.Logging/): `Install-Package Common.Logging` 

## Quickstart (built in console logger)
Nuget:

`PM> Install-Package Common.Logging` 

app.config or web.config:

	<?xml version="1.0" encoding="utf-8" ?>
	<configuration>
	
	  <configSections>
	    <sectionGroup name="common">
	      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
	    </sectionGroup>
	  </configSections>
	
	  <common>
		<logging>
		  <factoryAdapter type="Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging">
			<arg key="level" value="INFO" />
			<arg key="showLogName" value="true" />
			<arg key="showDataTime" value="true" />
			<arg key="dateTimeFormat" value="yyyy/MM/dd HH:mm:ss:fff" />
		  </factoryAdapter>
		</logging>
	  </common>

	</configuration>

## NLog Quickstart
Nuget:

    PM> Install-Package Common.Logging.NLog20 

app.config:

    <common>
    	<logging>
    	  <factoryAdapter type="Common.Logging.NLog.NLogLoggerFactoryAdapter, Common.Logging.NLog20">
    		<arg key="configType" value="INLINE" />
    	  </factoryAdapter>
    	</logging>
    </common>

    <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    ...
    </nlog>


See module documentation and [Source Forge](http://netcommon.sf.net/) for other available factoryAdapters and their configuration values


## Solution Layout

* "bin" contains the Common.Logging distribution dll files
* "doc" contains reference documentation.
* "shared" contains shared nant build scripts
* "modules" contains the sourcecode for Common.Logging.XXXX modules
* "redist" contains redistributable assemblies like log4net for your convenience

The Common Infrastructure Libraries for .NET are released under the terms of the Apache Software License (see license.txt).


## Building

* Clone the [github repository](https://github.com/net-commons/common-logging) 
* Install Silverlight SDK 5
* [Optional] Install Java for the documenation builder
* Create a strong name key. `c:\netcommon>sn -k common.net.snk`
* Build the the solution. `c:\netcommon>build-release.cmd`
