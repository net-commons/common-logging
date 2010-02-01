REM other targets are:
REM 'build'
REM 'test'
REM 'test-integration'

@ECHO OFF
cls

tools\nant\bin\nant.exe -f:Common.Logging.build package-zip
