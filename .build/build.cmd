@echo off
cd ..

python .build\build.py "calibre-search-app-windows.zip"
if %ERRORLEVEL% neq 0 goto :ExitPoint

echo Build completed successfully

:ExitPoint
cd .build
