using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBossKillMessage : MonoBehaviour
{
	public GameObject messageBox;

	// a simple function to close a message box
    public void CloseWindow()
    {
    	messageBox.SetActive(false);
    }
}
