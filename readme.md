# Common.Logging .NET

## Project Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/nyht5oguhan7gk2c/branch/master?svg=true)](https://ci.appveyor.com/project/sbohlen/common-logging/branch/master)

*Note: Build may periodically fail due to issues with the code-coverage tooling (NCover) being unable to reliably connect to the test-runner during test-runs on the CI server platform we've selected (Appveyor).  Work continues to investigate this further, but until resolved properly visitors are advised to discount failing builds reported here. 

## Introduction

Provides a simple logging abstraction to switch between different logging implementations.
There is current support for log4net, NLog, Microsoft Enterprise Library logging, Microsoft Application Insights, Microsoft Event Tracing for Windows, and Serilog.

Additionally Common.Logging comes with a set of base classes making the integration of any log system a breeze.

See also

* [Github Repo](http://github.com/net-commons/common-logging)
* [Project Website](http://net-commons.github.io/common-logging)
* [NuGet](https://www.nuget.org/packages/Common.Logging/): `Install-Package Common.Logging` 

## Console Quickstart
This demonstrates how to configure your app to log using the built in Common.Logging console adapter.

### 1) Install Common.Logging via NuGet
Either open the Package Management Console and enter the following or use the built-in GUI

NuGet: `PM> Install-Package Common.Logging` 

### 2) Register and configure Common.Logging
Then add the relevant sections to your `app.config` or `web.config`:

```xml
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
        <arg key="showDateTime" value="true" />
        <arg key="dateTimeFormat" value="yyyy/MM/dd HH:mm:ss:fff" />
      </factoryAdapter>
    </logging>
  </common>

</configuration>
```

## NLog Quickstart
There are different packages for each major NLog version. Install the correct package for your referenced NLog version. This example installs the adapter for NLog v4.1:

    PM> Install-Package Common.Logging.NLog41

If you are using NLog v4.0, you should install the `Common.Logging.NLog40` package and so on.  

NB: Because NLog is using semver and the same strong name for every major version, Common.Logging.NLog40 and Common.Logging.NLog41 works for all NLog 4.x version. Common.Logging.NLog41 is the recommend version

The app config should then have a common logging section like below. Be sure to match the `factoryAdapter type` with your installed `Common.Logging.NLogXX` version. 

```xml
<common>
    <logging>
      <factoryAdapter type="Common.Logging.NLog.NLogLoggerFactoryAdapter, Common.Logging.NLog41">
    	<arg key="configType" value="INLINE" />
      </factoryAdapter>
    </logging>
</common>

<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
...
</nlog>
```

## Application Insights Quickstart
There are different packages for each major Application Insights version. Install the correct package for your referenced Application Insights version. This example installs the adapter for Application Insights v0.17:

    PM> Install-Package Microsoft.ApplicationInsights -Version 0.17.0

The app config should then have an Application Insights section like below. Be sure to set the InstrumentationKey with your InstrumentationKey retrieved from Application Insights portal https://portal.azure.com.

```xml
<common>
    <logging>
      <factoryAdapter type="Common.Logging.ApplicationInsights.ApplicationInsightsLoggerFactoryAdapter, Common.Logging.ApplicationInsights">
    	<arg key="InstrumentationKey" value="[YOUR APPLICATION INSIGHTS INSTRUMENTATION KEY]" />
      </factoryAdapter>
    </logging>
</common>
```

See module documentation and [Source Forge](http://netcommon.sf.net/) for other factoryAdapters and their configuration values


## Solution Layout

* **bin** contains the Common.Logging distribution dll files
* **doc** contains reference documentation.
* **shared** contains shared nant build scripts
* **modules** contains the sourcecode for Common.Logging.XXXX modules
* **redist** contains redistributable assemblies like log4net for your convenience

The Common Infrastructure Libraries for .NET are released under the terms of the Apache Software License (see [license.txt](license.txt)).


## Building

* Clone the [GitHub repository](https://github.com/net-commons/common-logging) 
* Install Silverlight SDK 5
* [Optional] Install Java for the documentation builder
* Create a strong name key. `c:\netcommon>sn -k common.net.snk`
* Build the the solution. `c:\netcommon>build-release.cmd`
