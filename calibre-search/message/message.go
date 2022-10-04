package message

import (
	"bytes"
	"encoding/binary"
	"encoding/json"
	"os"
	"os/exec"
	"unsafe"
)

const VERSION = "0.2"

type ActionMessage struct {
	Action string `json:"action"`
	Search string `json:"search,omitempty"`
}

type ResultMessage struct {
	Status     string `json:"status"`
	Version    string `json:"version,omitempty"`
	ScriptPath string `json:"scriptPath,omitempty"`
}

var nativeEndian binary.ByteOrder

func initEndian() {
	buf := [2]byte{}
	*(*uint16)(unsafe.Pointer(&buf[0])) = uint16(0xABCD)

	switch buf {
	case [2]byte{0xCD, 0xAB}:
		nativeEndian = binary.LittleEndian
	case [2]byte{0xAB, 0xCD}:
		nativeEndian = binary.BigEndian
	default:
		panic("Could not determine native endianness.")
	}
}

func GetMessage() ActionMessage {
	initEndian()
	// Message starts with native unsigned integer (4 bytes)
	rawLengthBuffer := make([]byte, 4)
	_, err := os.Stdin.Read(rawLengthBuffer)
	if err != nil {
		os.Stderr.WriteString("Error reading length: " + err.Error())
	}
	rawLengthReader := bytes.NewReader(rawLengthBuffer)
	var messageLength uint32
	binary.Read(rawLengthReader, nativeEndian, &messageLength)

	messageBuffer := make([]byte, messageLength)
	_, err2 := os.Stdin.Read(messageBuffer)
	if err2 != nil {
		os.Stderr.WriteString("Error reading message: " + err2.Error())
	}
	actionMessage := ActionMessage{}
	json.Unmarshal(messageBuffer, &actionMessage)

	return actionMessage
}

func EncodeMessage(resultMessage ResultMessage) {
	data, _ := json.Marshal(resultMessage)
	dataText := string(data)
	// Write message length in first 4 bytes
	lengthBuffer := make([]byte, 4)
	length := (uint32)(len(dataText))
	nativeEndian.PutUint32(lengthBuffer, length)
	os.Stdout.Write(lengthBuffer)
	// Write JSON string
	os.Stdout.WriteString(dataText)
}

func SendTestResult() {
	resultMessage := ResultMessage{
		Status:  "Success",
		Version: VERSION,
	}
	path, _ := os.Getwd()
	resultMessage.ScriptPath = path
	EncodeMessage(resultMessage)
}

func DoSearch(search string) {
	resultMessage := ResultMessage{
		Status: "Success",
	}
	cmd := exec.Command("calibre", search)
	_, err := cmd.Output()

	if err != nil {
		os.Stderr.WriteString("Failed to launch calibre: " + err.Error())
		resultMessage.Status = "Failure"
	}
	EncodeMessage(resultMessage)
}
