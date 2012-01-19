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

tools\nant\bin\nant.exe -f:Common.Logging.build %1 %2 %3 %4 %5 %6 %7 %8 %9 > buildlog.txt

@echo ...complete!
@echo .
@echo .
@echo .

@echo Launching text file viewer to display buildlog.txt contents...
start "ignored but required placeholder window title argument" buildlog.txt
