REM other targets are:
REM 'build'
REM 'test'
REM 'test-integration'

@ECHO OFF
cls

@echo Building from script Common.Logging.build...
@echo .
@echo .
@echo .

rem tools\nant\bin\nant.exe -f:Common.Logging.build build package-zip package-nuget -D:project.sign=true -D:package.version=3.4.1 -D:nuget.version.suffix=RC1 -D:project.releasetype=release > buildlog.txt
tools\nant\bin\nant.exe -f:Common.Logging.build build package-zip package-nuget -D:project.sign=true -D:package.version=3.4.1 -D:project.releasetype=release > buildlog.txt

@echo ...complete!
@echo .
@echo .
@echo .

@echo Launching text file viewer to display buildlog.txt contents...
start "ignored but required placeholder window title argument" buildlog.txt