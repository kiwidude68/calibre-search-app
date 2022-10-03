#!/usr/bin/python -u
 
import json
import os
import sys
import struct
import subprocess

version = "1.1.0"

# Read a message from stdin and decode it.
def getMessage():
    raw_length = sys.stdin.buffer.read(4)

    if not raw_length:
        sys.exit(0)
    message_length = struct.unpack('=I', raw_length)[0]
    message = sys.stdin.buffer.read(message_length).decode("utf-8")
    return json.loads(message)

# Encode a message for transmission, given its content.
def encodeMessage(message_content):
    encoded_content = json.dumps(message_content).encode("utf-8")
    encoded_length = struct.pack('=I', len(encoded_content))
    #  use struct.pack("10s", bytes), to pack a string of the length of 10 characters
    return {'length': encoded_length, 'content': struct.pack(str(len(encoded_content))+"s",encoded_content)}

# Send an encoded message to stdout.
def sendMessage(encoded_message):
    sys.stdout.buffer.write(encoded_message['length'])
    sys.stdout.buffer.write(encoded_message['content'])
    sys.stdout.buffer.flush()

message = getMessage()
result = {
    'status': 'Failure'
}
if message['action'] == 'TEST_CONNECTIVITY':
    result['status'] = 'Success'
    result['version'] = version
    scriptPath = os.path.dirname(os.path.abspath(__file__))
    result['scriptpath'] = scriptPath + os.sep
    sendMessage(encodeMessage(json.dumps(result)))
else:
    search = message['search']
    try:
        subprocess.run(["calibre", search])
        result['status'] = 'Success'
    except Exception as e:
        print('Could not send message to calibre. ({})'.format(e))
    sendMessage(encodeMessage(json.dumps(result)))
