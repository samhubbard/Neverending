using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomBelowScript : MonoBehaviour {

	// this script is only used for my testing purposes.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            SceneManager.LoadScene("DungeonLevel_Boss");
        }
    }
}
