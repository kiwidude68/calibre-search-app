#!/usr/bin/python

__license__   = 'GPL v3'
__copyright__ = '2022, Grant Drake'

'''
Creates a GitHub release for calibre-search-app, including uploading the zip file.

Invocation should be via release.cmd, which will ensure that:
- Working directory is set to the root subfolder
- Zip file is rebuilt for latest local code
- Pass through the CALIBRE_GITHUB_TOKEN environment variable value
'''

import sys, os, re, json
from urllib import request, error

API_REPO_URL = 'https://api.github.com/repos/kiwidude68/calibre-search-app'
ZIP_NAME = 'calibre-search-app.zip'
APP_RELEASE_NAME = 'Calibre Search App'

def getZipPath():
    zipFile = os.path.join(os.getcwd(), ZIP_NAME)
    if not os.path.exists(zipFile):
        print('ERROR: No zip file found at: {}'.format(zipFile))
        raise FileNotFoundError(zipFile)
    return zipFile

def readLatestChangeLog():
    changeLogFile = os.path.join(os.getcwd(), 'CHANGELOG.md')
    if not os.path.exists(changeLogFile):
        print('ERROR: No change log found at: {}'.format(changeLogFile))
        raise FileNotFoundError(changeLogFile)
    
    with open(changeLogFile, 'r') as file:
        content = file.readlines()
    
    foundVersion = False
    version = None
    changeLines = []
    for line in content:
        if not foundVersion:
            if line.startswith('## ['):
                foundVersion = True
                versionMatch = re.match('##\s\[([\d\.]+)\]', line)
                if versionMatch:
                    version = versionMatch.groups(0)[0]
                    print('Found version: {}'.format(version))
                else:
                    print('ERROR:No version found in the changelog file')
                    raise RuntimeError('Missing version in changelog')
            continue
        # We are within the current version - include content unless we hit the previous version
        if line.startswith('## ['):
            break
        changeLines.append(line)

    if len(changeLines) == 0:
        print('ERROR: No change log details found')
        raise RuntimeError('Missing details in changelog')

    # Trim trailing blank lines
    while changeLines and len(changeLines[-1]) <= 2:
        changeLines.pop()

    print('ChangeLog details found: {0} lines'.format(len(changeLines)))
    return (version, ''.join(changeLines))

def checkIfReleaseExists(apiToken, tagName):
    # If we have already released this version then we have a problem
    # Most likely have forgotten to bump the version number in changelog?
    endpoint = API_REPO_URL + '/releases/tags/' + tagName
    req = request.Request(url=endpoint, method='GET')
    req.add_header('accept', 'application/vnd.github+json')
    req.add_header('Authorization', 'BEARER {}'.format(apiToken))
    try:
        print('Checking if GitHub tag exists: {}'.format(endpoint))
        with request.urlopen(req) as response:
            response = response.read().decode('utf-8')
            raise RuntimeError('Release for this version already exists. Do you need to bump version?')
    except error.HTTPError as e:
        if e.code == 404:
            print('Existing release for this version not found, OK to proceed')
        else:
            raise RuntimeError('Failed to check release existing API due to:',e)

def createGitHubRelease(apiToken, tagName, changeBody):
    endpoint = API_REPO_URL + '/releases'
    data = {
        'tag_name': tagName,
        'target_commitish': 'main',
        'name': '{} v{}'.format(APP_RELEASE_NAME, version),
        'body': changeBody,
        'draft': False,
        'prerelease': False,
        'generate_release_notes': False
    }
    data = json.dumps(data)
    data = data.encode()
    req = request.Request(url=endpoint, data=data, method='POST')
    req.add_header('accept', 'application/vnd.github+json')
    req.add_header('Authorization', 'BEARER {}'.format(apiToken))
    req.add_header('Content-Type', 'application/json')
    try:
        print('Creating release: {}'.format(endpoint))
        with request.urlopen(req) as response:
            response = response.read().decode('utf-8')
            content = json.loads(response)
            htmlUrl = content['html_url']
            uploadUrl = content['upload_url']
            return (htmlUrl, uploadUrl)
    except error.HTTPError as e:
        raise RuntimeError('Failed to create release due to:',e)

def uploadZipToRelease(apiToken, uploadUrl, zipFile):
    downloadZipName = ZIP_NAME
    endpoint = uploadUrl.replace('{?name,label}','?name={}&label={}'.format(downloadZipName, downloadZipName))
    with open(zipFile, 'rb') as file:
        content = file.read()

    req = request.Request(url=endpoint, data=content, method='POST')
    req.add_header('accept', 'application/vnd.github+json')
    req.add_header('Authorization', 'BEARER {}'.format(apiToken))
    req.add_header('Content-Type', 'application/octet-stream')
    try:
        print('Uploading zip for release: {}'.format(endpoint))
        with request.urlopen(req) as response:
            response = response.read().decode('utf-8')
            content = json.loads(response)
            downloadUrl = content['browser_download_url']
            print('Zip uploaded successfully: {}'.format(downloadUrl))
    except error.HTTPError as e:
        raise RuntimeError('Failed to upload zip due to:',e)


if __name__=="__main__":
    
    apiToken = sys.argv[1]
    if not apiToken:
        raise RuntimeError('No GitHub API token found. Please set it in CALIBRE_GITHUB_TOKEN variable.')

    (version, changeBody) = readLatestChangeLog()
    tagName = 'v' + version
    zipFile = getZipPath()

    checkIfReleaseExists(apiToken, tagName)
    (htmlUrl, uploadUrl) = createGitHubRelease(apiToken, tagName, changeBody)
    uploadZipToRelease(apiToken, uploadUrl, zipFile)
    
    print('Github release completed: {}'.format(htmlUrl))