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

tools\nant\bin\nant.exe -f:Common.Logging.build package-zip -D:project.sign=true -D:project.version=2.0.0.0 -D:project.releasetype=release > buildlog.txt

@echo ...complete!
@echo .
@echo .
@echo .

@echo Launching text file viewer to display buildlog.txt contents...
start "ignored but required placeholder window title argument" buildlog.txt