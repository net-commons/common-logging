Spring.NHiberate, release 1.0.0 (April 27 2005)
---------------------------------------------------------
http://www.springframework.net/


1. INTRODUCTION


2. KNOWN ISSUES

3. RELEASE INFO

Release contents:

* "src" contains the C# source files for the framework
* "test" contains the C# source files for Spring.NET's test suite
* "bin" contains various Spring.NET distribution dll files
* "lib" contains all third-party libraries needed for building the framework
* "doc" contains reference documentation, MSDN-style API help, and the Spring.NET xsd.
* "examples" contains sample applications.
* "install" contains the installation project

The VS.NET solution for the framework and examples are provided.  

Latest info is available at the public website: http://www.springframework.net/

Project info at the SourceForge site: http://sourceforge.net/projects/springnet/

The Spring Framework is released under the terms of the Apache Software License (see license.txt).


4. DISTRIBUTION DLLs

The "bin" directory contains the following distinct dll files for use in applications. Dependencies are those other than on the .NET BCL.

* "Spring.Core" (~412 KB)
- Contents: Inversion of control container. Collection classes. Object expression language.
- Dependencies: Log4Net, ANTLR

* "Spring.Aop" (~96 KB)
- Contents: Abstract Oriented Programming Framework.
- Dependencies: Spring.Core, Log4Net


5. WHERE TO START?

Documentation can be found in the "docs" directory:
* The Spring reference documentation

Documented sample applications can be found in "samples":
* MovieFinder - A simple example demonstrating basic IoC container behavior.
* AppContext - Show use of various IApplicationContext features.
* EventRegistry - Show use of loosely coupled eventing features.
* AopQuickStart - Show use of AOP features.


6. How to build

VS.NET
------
There is a solution file, Spring.Net.1.0.sln, for use with VS.NET 2003 and
a solution file, Spring.Net.1.0.2005.sln for use with VS.NET 2005

You can add intellisence support for authoring Spring.NET XML configuraiton files
by registering the spring-objects.xsd schema with VS.NET.  An NAnt script
in the directory doc/schema will register the schema with VS.NET 2002,2003,2005.
If you are upgrading from an older version you will need to delete the older
versions installed under the appropriate VS.NET directory.

NAnt
----

A NAnt build script can be obtained via CVS or nightly builds.  The
version of NAnt used to build is the 4/30/2006 nightly build as it support
.NET 2.0 and NUnit 2.2.7

To build the source and run the unit tests type

nant test

Release builds are strongly named using the Spring.Net.snk key file.  The ANTLR assemblies
are signed with this key as well until such time as the ANTR distribution provides their
own strongly signed assemblies.  If you want to run the release build you can generate a 
key file by executing the following commands

sn -k Spring.Net.snk
nant

The installation of the docbook toolchain is required to generate the reference
documentation and make a final release build. Refer to the Spring.NET Wiki at
http://opensource.atlassian.com/confluence/spring/display/NET/Docbook+Reference
for information on how to install the docbook toolchain.

Innovasys Document X! is used to generate the SDK documentation.


7. Support

The user forms at http://forum.springframework.net/ are available for you to submit questions, support requests, and interact with other Spring.NET users.

Bug and issue tracking can be found at http://opensource.atlassian.com/projects/spring/secure/BrowseProject.jspa?id=10020

The Spring.NET wiki is located at http://opensource.atlassian.com/confluence/spring/display/NET/Home

A Fisheye repository browser is located at http://fisheye1.cenqua.com/viewrep/springnet

8. Acknowledgements

InnovaSys Document X!
---------------------
InnovSys has kindly provided a license to generate the SDK documentation and supporting utilities for
integration with Visual Studio. 








