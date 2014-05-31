REM other targets are:
REM 'build'
REM 'test'
REM 'test-integration'

@ECHO OFF
tools\nant\bin\nant.exe -f:Common.Logging.build test-integration > buildlog.txt