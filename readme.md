# Common.Logging .NET

## Project Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/nyht5oguhan7gk2c/branch/master?svg=true)](https://ci.appveyor.com/project/sbohlen/common-logging/branch/master)


## Introduction

Provides a simple logging abstraction to switch between different logging implementations.
There is current support for log4net, NLog and Enterprise Library logging.

Additionally Common.Logging comes with a set of base classes making the integration of any log
system a breeze.

See also

* [Github](http://github.com/net-commons/common-logging)
* [Source Forge](http://netcommon.sf.net/)
* [Nuget](https://www.nuget.org/packages/Common.Logging/): `Install-Package Common.Logging` 

## Console Quickstart
This demonstrates how to configure your app to log using the built in Common.Logging console adapter.
 
Nuget: `PM> Install-Package Common.Logging` 

Then add the relevant sections to your app.config or web.config:

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
There are different packages for each major NLog version. Install the correct package for your referenced NLog version. This example installs the adapter for Nog 2.0:

    PM> Install-Package Common.Logging.NLog20

If you are using NLog 3.0, you should install the `Common.Logging.NLog30` package and so on.  

The app config should then have a common logging section like below. Be sure to match the `factoryAdapter type` with your installed `Common.Logging.NLogXX` version. 

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


See module documentation and [Source Forge](http://netcommon.sf.net/) for other factoryAdapters and their configuration values


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
