#REM Install service can be done with ProcessStarterService.exe -i
echo off
set "servicePath=%~dp0Service\bin\Debug\ProcessStarterService.exe"
set "serviceName=ProcessStarterServiceDebug"
set "displayName=Process Starter Service Debug"
set "description=Service that starts process at specified interval with specified arguments"
call sc.exe create %serviceName% DisplayName= "%displayName%" start= "delayed-auto" binPath= "%servicePath%"
call sc.exe description %serviceName% "%description%"
pause