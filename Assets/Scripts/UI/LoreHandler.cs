using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreHandler : MonoBehaviour
{
    public GameObject loreWindow;
    public Text entryOne;
    public Text entryTwo;
    public Text entryThree;
    public Text entryFour;

    // get and setup what lore will be displayed based on the player's longest streak
    private void Start()
    {
    	if (PlayerPrefs.GetInt("StreakOfOneCompleted") == 1)
    	{
    		entryOne.text = "This place seems strange. There is only a portal that leads to an ever changing dungeon.";
    	}
    	else
    	{
    		int currentBestStreak = PlayerPrefs.GetInt("CurrentBestStreak");
    		string progress = "Complete one dungeon run to unlock this lore entry.\n(Current best streak: " + currentBestStreak + ")";
    		entryOne.text = progress;
    	}

    	if (PlayerPrefs.GetInt("StreakOfFiveCompleted") == 1)
    	{
    		entryTwo.text = "Funny... I don't get hungry or thirsty anymore. It appears that the only thing to do is to go into that dungeon. ";
    	}
    	else
    	{
    		int currentBestStreak = PlayerPrefs.GetInt("CurrentBestStreak");
    		string progress = "Complete five dungeon runs in one life to unlock this lore entry.\n(Current best streak: " + currentBestStreak + ")";
    		entryTwo.text = progress;
    	}

    	if (PlayerPrefs.GetInt("StreakOfTwentyCompleted") == 1)
    	{
    		entryThree.text = "Is there anything else? I almost look forward to dying in that dungeon just so that I can get a new setup going. That little variety is what keeps me going.";
    	}
    	else
    	{
    		int currentBestStreak = PlayerPrefs.GetInt("CurrentBestStreak");
    		string progress = "Complete twenty dungeon runs in one life to unlock this lore entry.\n(Current best streak: " + currentBestStreak + ")";
    		entryThree.text = progress;
    	}

    	if (PlayerPrefs.GetInt("StreakOfFiftyCompleted") == 1)
    	{
    		entryFour.text = "Yep, I'm dead. This is my personal hell. I am to repeat run this dungeon for all eternity and my only solice is to see if I can beat my personal best streak.";
    	}
    	else
    	{
    		int currentBestStreak = PlayerPrefs.GetInt("CurrentBestStreak");
    		string progress = "Complete fifty dungeon runs in one life to unlock this lore entry.\n(Current best streak: " + currentBestStreak + ")";
    		entryFour.text = progress;
    	}
    }

    // show the lore menu
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if (other.CompareTag("Player"))
    	{
    		loreWindow.SetActive(true);
    	}
    }

    // hide lore menu
    private void OnTriggerExit2D(Collider2D other)
    {
    	if (other.CompareTag("Player"))
    	{
    		loreWindow.SetActive(false);
    	}
    }
}
