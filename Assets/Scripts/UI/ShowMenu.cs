using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
	public GameObject menu;

	// shows the selected menu
    public void Show()
    {
        Time.timeScale = 0;
    	menu.SetActive(true);
    }
}