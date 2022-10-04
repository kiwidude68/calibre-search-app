@echo off
pushd ..\calibre-search

go build
echo Go build completed successfully

copy calibre-search.exe %localappdata%\\com.kiwidude.calibre_search\\

:ExitPoint
popd
