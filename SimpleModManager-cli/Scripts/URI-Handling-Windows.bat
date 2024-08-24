@echo off
setlocal

:: Check if the application path is provided as an argument
if "%~1"=="" (
    echo Usage: %0 "path\to\application.exe"
    exit /b 1
)

:: Set the URI scheme and application path variables
set "schemeName=nxm"
set "applicationPath=%~1"

:: Register the custom URI scheme in the Windows Registry
reg add "HKCU\Software\Classes\%schemeName%" /ve /d "URL:%schemeName% Protocol" /f
reg add "HKCU\Software\Classes\%schemeName%" /v "URL Protocol" /f
reg add "HKCU\Software\Classes\%schemeName%\shell\open\command" /ve /d "\"%applicationPath%\" \"%%1\"" /f

echo %schemeName% protocol handler registered successfully.

endlocal
exit /b 0
