@echo off
call build.cmd

cd ..

python .\.build\release.py "%CALIBRE_GITHUB_TOKEN%"

cd .build
