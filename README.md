# Calibre Search Native Application
[![History][changelog-image]][changelog-url] 
[![License][license-image]][license-url]

## Introduction

This is the supporting application required for the [calibre-search-chrome][calibre-search-chrome-url] and [calibre-search-firefox][calibre-search-firefox-url] web extensions.

These extensions allow you to quickly perform a search against your local [calibre][calibre-url] library based on the selected text or links on a web page.

For more information, please refer to the [FAQ][wiki-url]

## Python based calibre-search.py

The default installation requires a Python script `calibre-search.py` for the implementation of the native application. It has a wrapper `calibre-search.cmd` for ease of execution on Windows.

The downside of this approach is it requires the user to have Python 3.5+ installed on their machine/in their `PATH`.

## Go based calibre-search.exe

Just for funsies I created a Go-based application that does the equivalent of `calibre-search.py` and `calibre-search.cmd`. It compiles into a single executable called `calibre-search.exe`.

The advantage for a user with this approach is that it means they do not have to have Python 3.5+ installed on their machine/in their `PATH`.

> The one negative of an executable is that it will get detected by Windows resulting in a `Security scan required` message. It won't actually find anything wrong (nor should it!), the functionality will work ok but some users may get freaked out by it.

I will package this up as another option for users soon once I add an icon resource/metadata and update the build/release scripts...

## .NET 6 based CalibreSearchApp.Tester

This .NET 6.0 project is for verifying the host application setup. It actually has several purposes:
- Allows an end user to troubleshoot their Chrome or Firefox host application installation.
- Provided a handy test harness for developing the golang executable above.

This tester executable allows you to verify:
- The registry keys are present and pointing to a manifest `.json` file
- The manifest `.json` file is present and is valid json
- The manifest file has a `path` field pointing to the executable.
- The executable is present.
- The executable responds correctly to `stdin` messages and with expected output.

You can also invoke a search against your local calibre library to prove that works too (if you have calibre running).

Since I am not expecting any ongoing issues I am not planning to offer this as a built executable at the moment. However the code is here if the need for troubleshooting arises again.

## Donations

If you enjoy my calibre plugins or extensions, please feel free to show your appreciation!

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)][donate-url]

[paypal.me/kiwidudeOz][donate-url]

[donate-url]: https://paypal.me/kiwidudeOz
[wiki-url]: https://github.com/kiwidude68/calibre-search-app/wiki/Calibre-Search-FAQ

[calibre-url]: https://calibre-ebook.com/
[calibre-search-chrome-url]: https://github.com/kiwidude68/calibre-search-chrome
[calibre-search-firefox-url]: https://github.com/kiwidude68/calibre-search-firefox

[changelog-image]: https://img.shields.io/badge/History-CHANGELOG-blue.svg
[changelog-url]: CHANGELOG.md

[license-image]: https://img.shields.io/badge/License-GPL-yellow.svg
[license-url]: ../LICENSE.md
