using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour {

    GameController gameController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            // this ensures that the player has completed the room event prior to heading off
            if (!RoomEvent.roomEventActive)
            {
                // increases the current level that the player is on
                gameController.playerStats.CurrentLevel++;
                gameController.playerStats.RoomsClearedCounter = 0;
                gameController.SavePlayerProgress();
                
                SceneManager.LoadScene(gameController.playerStats.CurrentLevel);
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
            }
            else
            {
                // inform the player that they need to complete the room prior to progressing
                string messageToSend = "You must clear the room before continuing.";
                DisplayMessage.MessageToQueue(messageToSend);
            }
        }
    }

    void Start () {
        // link in the game controller
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
}
