REM other targets are:
REM 'build'
REM 'test'
REM 'test-integration'

tools\nant\bin\nant.exe -f:Common.Logging.build test-integration -D:buildconfigflag.release=false