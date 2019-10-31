using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseWelcomeMessage : MonoBehaviour
{
	public GameObject welcomeMessage;
	public GameObject classSelection;
	public Text messageToPlayer;

    public void CloseWindow()
    {
        welcomeMessage.SetActive(false);
        PlayerController.disabled = true;
        classSelection.SetActive(true);
        messageToPlayer.text = "Pick your class and get going!";
    }
}
