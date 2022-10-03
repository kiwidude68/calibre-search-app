@ECHO OFF

SET PATH=C:\Windows\System32;%PATH%

IF EXIST "%~dp0\app\calibre-search.py" (GOTO :EXISTING) ELSE GOTO :MISSING

:EXISTING
  ECHO .. Writing to Chrome Registry
  ECHO .. Key: HKCU\Software\Google\Chrome\NativeMessagingHosts\com.kiwidude.calibre_search
  REG ADD "HKCU\Software\Google\Chrome\NativeMessagingHosts\com.kiwidude.calibre_search" /ve /t REG_SZ /d "%LOCALAPPDATA%\com.kiwidude.calibre_search\manifest-chrome.json" /f

  ECHO .. Writing to Chromium Registry
  ECHO .. Key: HKCU\Software\Chromium\NativeMessagingHosts\com.kiwidude.calibre_search
  REG ADD "HKCU\Software\Chromium\NativeMessagingHosts\com.kiwidude.calibre_search" /ve /t REG_SZ /d "%LOCALAPPDATA%\com.kiwidude.calibre_search\manifest-chrome.json" /f

  ECHO .. Writing to Edge Registry
  ECHO .. Key: HKCU\Software\Microsoft\Edge\NativeMessagingHosts\com.kiwidude.calibre_search
  REG ADD "HKCU\Software\Microsoft\Edge\NativeMessagingHosts\com.kiwidude.calibre_search" /ve /t REG_SZ /d "%LOCALAPPDATA%\com.kiwidude.calibre_search\manifest-chrome.json" /f

  ECHO .. Writing to Firefox Registry
  ECHO .. Key: HKCU\SOFTWARE\Mozilla\NativeMessagingHosts\com.kiwidude.calibre_search
  REG ADD "HKCU\SOFTWARE\Mozilla\NativeMessagingHosts\com.kiwidude.calibre_search" /ve /t REG_SZ /d "%LOCALAPPDATA%\com.kiwidude.calibre_search\manifest-firefox.json" /f

  ECHO .. Writing to Waterfox Registry
  ECHO .. Key: HKCU\SOFTWARE\Waterfox\NativeMessagingHosts\com.kiwidude.calibre_search
  REG ADD "HKCU\SOFTWARE\Waterfox\NativeMessagingHosts\com.kiwidude.calibre_search" /ve /t REG_SZ /d "%LOCALAPPDATA%\com.kiwidude.calibre_search\manifest-firefox.json" /f

  ECHO .. Writing to Thunderbird Registry
  ECHO .. Key: HKCU\SOFTWARE\Thunderbird\NativeMessagingHosts\com.kiwidude.calibre_search
  REG ADD "HKCU\SOFTWARE\Thunderbird\NativeMessagingHosts\com.kiwidude.calibre_search" /ve /t REG_SZ /d "%LOCALAPPDATA%\com.kiwidude.calibre_search\manifest-firefox.json" /f

  PUSHD "%~dp0"
  CD app

  SET EXT_DIR=%LOCALAPPDATA%\com.kiwidude.calibre_search
  ECHO .. Copying host files to: %EXT_DIR%

  IF NOT EXIST "%EXT_DIR%" (
    MD %EXT_DIR%
  )
  XCOPY "*.*" "%EXT_DIR%" /K /Y

  ECHO .. Setting manifest path locations
  
  SET MANIFESTPATH=%EXT_DIR%\calibre-search.cmd
  SET MANIFESTPATH=%MANIFESTPATH:\=\\%
  ECHO %MANIFESTPATH%
  FOR %%f IN (%EXT_DIR%\manifest-*.json) DO (
    CALL :MANIFEST_SET_PATH %%f
  )
  SET MANIFESTPATH=
  
  GOTO :COMMON

:MISSING
  ECHO To run the installer, please first unzip the archive

:COMMON
  ECHO Press ENTER key to exit
  PAUSE
  EXIT /b

:MANIFEST_SET_PATH
  ECHO Updating manifest path in: %1
  SET "textfile=%1"
  SET "tempfile=%1.tmp"
  (FOR /F "delims=" %%i IN (%textfile%) DO (
    SET "line=%%i"
    SETLOCAL ENABLEDELAYEDEXPANSION
    SET "line=!line:MANIFESTPATH=%MANIFESTPATH%!"
    ECHO(!line!
    ENDLOCAL
  ))>"%tempfile%"
  DEL %textfile%
  FOR /F "delims=" %%i IN ("%textfile%") DO SET textfileonly=%%~nxf
  RENAME %tempfile% %textfileonly%
  EXIT /b