REM other targets are:
REM 'build'
REM 'test'
REM 'test-integration'

@ECHO OFF
call build package-zip -D:project.sign=true -D:project.version=2.0.0.0 -D:project.releasetype=release