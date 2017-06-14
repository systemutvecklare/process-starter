#REM Install service can be done with ProcessStarterService.exe -u
echo off
set "serviceName=ProcessStarterServiceDebug"
call sc.exe delete %serviceName%
pause