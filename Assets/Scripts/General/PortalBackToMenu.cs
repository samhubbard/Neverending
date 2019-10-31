using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBackToMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    private float timer;

    private void Start()
    {
        GetComponent<Animator>().SetTrigger("PortalSpawn");
        timer = 1.0f;
        
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && timer <= 0)
        {
            // take the player back to the main menu after killing a boss
            Instantiate(loadingScreen, transform.position, Quaternion.identity);
            Destroy(GameObject.Find("GameController"));
            SceneManager.LoadScene(0);
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }
    }
}
