using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMessage : MonoBehaviour
{
    Queue<MessageDisplay> messageDisplayRequest = new Queue<MessageDisplay>();
    MessageDisplay currentMessageToDisplay;

    static DisplayMessage instance;

    bool isProcessingMessage;

    private Text displayText;
    private float textDisplayDuration = 1.5f;

    // sets the instance of this object
    private void Awake()
    {
        instance = this;

    }

    // adds in a new message to add to the queue to show to the player
    public static void MessageToQueue(string messageToDisplay)
    {
        MessageDisplay newMessageRequest = new MessageDisplay(messageToDisplay);
        instance.messageDisplayRequest.Enqueue(newMessageRequest);
        instance.TryProcessNext();
    }

    // this takes the next in the queue and attempts to display it
    void TryProcessNext()
    {
        if (!isProcessingMessage && messageDisplayRequest.Count > 0)
        {
            currentMessageToDisplay = messageDisplayRequest.Dequeue();
            isProcessingMessage = true;
            FlashTheMessage(currentMessageToDisplay.messageToDisplay);
        }
    }

    // flashes the message on to the screen
    void FlashTheMessage(string _message)
    {
        displayText = GetComponent<Text>();
        displayText.text = _message;
        Invoke("ClearDisplay", textDisplayDuration);
    }

    // clears the text from the flash area
    void ClearDisplay()
    {
        displayText.text = "";
        isProcessingMessage = false;
        TryProcessNext();
    }

    // this struct handles the message display
    struct MessageDisplay
    {
        public string messageToDisplay;

        public MessageDisplay(string _messageToDisplay)
        {
            messageToDisplay = _messageToDisplay;
        }
    }
}
