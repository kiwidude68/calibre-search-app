# Calibre Search Native Application
[![History][changelog-image]][changelog-url] 
[![License][license-image]][license-url]

## Introduction

This is the supporting application required for the [calibre-search-chrome][calibre-search-chrome-url] and [calibre-search-firefox][calibre-search-firefox-url] web extensions.

These extensions allow you to quickly perform a search against your local [calibre][calibre-url] library based on the selected text or links on a web page.

In order to pass the search text from your web browser to calibre, a **native application** is required in between. This application acts as a proxy between the secure world of your web browser and your desktop computer. You can see this described with a nice diagram in the first link below:

- [Firefox Native Messaging][native-firefox-url]
- [Chrome Native Messaging][native-chrome-url]

For this implementation we have a relatively simple Python script, which listens for a message from the calibre-search web extension containing the text to search. This Python script can then run a calibre command to display your search results in your library using [URL scheme syntax][calibre-url-scheme-url] added in calibre v5.5.

## What else do I need?

- [Python v3][python-url] installed, and in your `PATH`
- [calibre v5.5 or later][calibre-url] running to display your results!

## What browsers are supported?

A user only needs to install this application once, and _theoretically_ includes support for all of these browsers:
- Chrome
- Chromium
- Edge
- Firefox
- Waterfox
- Thunderbird

I say _theoretically_ because I don't personally have the necessary systems to test the web extensions against all of those.

## What does the calibre-search-app consist of?

Instructions will vary by platform, but describing the most common case of Windows users:
- `install-host.bat` batch file a user can run (as administrator) to install this app in one step.
- Registry key entries to point to the browser specific `manifest-*.json` file below.
- `manifest-chrome.json` and `manifest-firefox.json` files, describing the command lines to run.
- `calibre-search.cmd` batch file invoked every time you do a search from the extension to run the Python script.
- `calibre-search.py` Python script that listens for the message from the browser and launches the calibre search.

## Where does it install to?

On **Windows** if you run `install-host.cmd` then all the necessary files will be copied to: `%LOCALAPPDATA%\com.kiwidude.calibre-search\`

If you don't like this location you can of course move things around, but you must make sure the registry keys and the paths within the `manifest-*.json` files reflect the location.

## Why Python?

Could I have chosen a different tech stack? Of course. However as a personal project with Python already installed it was super quick to get up and running.

A couple of other options I considered (and may yet offer in future) are listed below:
- Create [nodejs](https://nodejs.org/) scripts, and bundle nodejs with this code. That would negate the need for the user to have Python installed/in their path. 
    - Using Python does avoid the need for putting nodejs binaries in my repo and dealing with OS platform builds.
- Build a custom executable using [Golang](https://go.dev/). This would give a very nice small executable, not requiring other runtime support like Python or nodejs. 
    - Would users "trust" an executable over a script they can open and read for themselves?
    - It would however be a fun personal learning exercise I may yet do.

It should be said neither of those approaches avoids the need for the registry keys, manifest files and some form of installation step. They just avoid the need for a user to install Python.

## Can we avoid a native application completely?

This was a suggestion by the calibre creator Kovid Goyal himself on the [MobileRead forums][mobileread-dev-thread-url]. By adding a specific new endpoint to the calibre Content Server, we could have the browser extensions send an http request direct to calibre rather than using the native messaging approach above. The Content Server code would do the equivalent of the Python script above of running a search in calibre.

So no need for this application/separate download, no multi-platform issues - sounds amazing right? Indeed it would be, however...

1. Someone has to develop this new endpoint, and get it included with a calibre build.
    - Forces users to be running the latest calibre version.
    - Requires calibre source code knowledge I do not have currently.
2. Each user must have Content Server running
    - Within the gui there is an option to _Run server automatically when calibre starts_
    - Are there downsides to this like adding to calibre startup/shutdown times?
3. It is yet to be tested that we could satisfactorily invoke this from extension code.
    - I _think_ it could be done using `fetch()` - see [fetch documentation](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch)

Never say never on this approach if time allows or someone else volunteers. I could always make it optional in the extensions options pages so a user not wanting to run Content Server could still use this native application approach.

[python-url]: https://www.python.org/downloads/

[calibre-url]: https://calibre-ebook.com/
[calibre-url-scheme-url]: https://manual.calibre-ebook.com/url_scheme.html
[calibre-search-chrome-url]: https://github.com/kiwidude68/calibre-search-chrome
[calibre-search-firefox-url]: https://github.com/kiwidude68/calibre-search-firefox

[native-firefox-url]: https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions/Native_messaging
[native-chrome-url]: https://developer.chrome.com/docs/apps/nativeMessaging/

[mobileread-dev-thread-url]: https://www.mobileread.com/forums/showthread.php?t=349353

[changelog-image]: https://img.shields.io/badge/History-CHANGELOG-blue.svg
[changelog-url]: CHANGELOG.md

[license-image]: https://img.shields.io/badge/License-GPL-yellow.svg
[license-url]: ../LICENSE.md
