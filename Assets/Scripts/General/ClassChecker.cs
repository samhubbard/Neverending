using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassChecker : MonoBehaviour
{
    public GameObject classSelection;
    public GameObject postBossKillMessage;
    public GameObject welcomeMessage;
    public Text messageToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.disabled)
        {
            PlayerController.disabled = false;
        }
        // check to see if the player died in their last run
        PlayerStats character = GetComponent<PlayerStatHandler>().GetPlayerStats();

        if (PlayerPrefs.GetString("WelcomeMessageViewed") != "true")
        {
        	welcomeMessage.SetActive(true);
            PlayerPrefs.SetString("WelcomeMessageViewed", "true");
        }
        else
        {
        	if (character.FlaggedForDeletion)
        	{
        		classSelection.SetActive(true);
        		PlayerController.disabled = true;
        		messageToPlayer.text = "Sorry you died. Try again!";
        	}
        	else if (PlayerPrefs.GetString("JustKilledBoss") == "true")
        	{
        		postBossKillMessage.SetActive(true);
        		PlayerPrefs.SetString("JustKilledBoss", "false");
        	}
        }
    }
}
