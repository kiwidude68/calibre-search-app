package main

import (
	message "kiwidude.com/calibre-search/message"
)

func main() {
	result := message.GetMessage()

	if result.Action == "TEST_CONNECTIVITY" {
		message.SendTestResult()
	} else {
		message.DoSearch(result.Search)
	}
}
