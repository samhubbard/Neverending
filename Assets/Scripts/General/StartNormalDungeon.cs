using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNormalDungeon : MonoBehaviour {

    public GameObject loadingScreen;
    private PlayerStats player;

    private void Start() 
    {
        player = GetComponent<PlayerStatHandler>().GetPlayerStats();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // stop the audio, show the loading screen, and load into the dungeon
            AudioStop();
            ShowLoadingScreen();
            Invoke("LoadScene", 0.5f);
        }
    }

    private void AudioStop()
    {
        AudioSource footSteps = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        footSteps.Stop();
        AudioSource music = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        music.Stop();
    }

    private void ShowLoadingScreen()
    {
        // instantiate the loading screen into the scene
        GameObject screen = Instantiate(loadingScreen, transform.position, Quaternion.identity) as GameObject;
        screen.transform.SetParent(GameObject.FindWithTag("LoadingScreen").transform, false);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(player.CurrentLevel);
    }
}
