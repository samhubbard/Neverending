using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceDrop : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int currentPortalCount = PlayerPrefs.GetInt("NormalPortalCount");
            print("Incoming portal count: " + currentPortalCount);
            currentPortalCount += 1;
            PlayerPrefs.SetInt("NormalPortalCount", currentPortalCount);
            print("Updated portal count: " + currentPortalCount);

            Destroy(GameObject.Find("GameController"));
            SceneManager.LoadScene(0);
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }
    }
}
