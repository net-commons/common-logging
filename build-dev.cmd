REM other targets are:
REM 'build'
REM 'test'
REM 'test-integration'

@ECHO OFF
cls
rem rd /S /Q build

tools\nant\bin\nant.exe -f:Common.Logging.build package
