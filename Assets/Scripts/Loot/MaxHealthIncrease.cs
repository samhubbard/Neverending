using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthIncrease : MonoBehaviour
{
    public AudioClip soundEffect;
    private bool canDestroy;
    private bool hasDestroyed;
    private bool hasPickedup;
    private float timer;

    // sets up the timer and all of the flags for functionality
    void Start()
    {
        canDestroy = false;
        hasDestroyed = false;
        hasPickedup = false;
        timer = 0.5f;
    }

    // keeps tabs on the timer and eventually destroys the object from the scene
    void Update()
    {
        if (timer <= 0 && hasPickedup)
        {
            canDestroy = true;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (!hasDestroyed && canDestroy)
        {
            hasDestroyed = true;
            Destroy(gameObject.transform.parent.gameObject, 2.0f);
            Destroy(gameObject);
        }
    }

    // once the player runs over it
    // plays the pickup sound
    // updates the player stats and informs the player of what was increased
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPickedup)
        {
            AudioSource audioSource = gameObject.transform.parent.gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffect;
            audioSource.volume = 1.0f * GameController.sfxVolume;

            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

            GameController.maxHealth += 25;
            gameController.playerStats.TotalHealth += 25;
            gameController.UpdatePlayerStats();
            //gameController.healthBar.maxValue = GameController.maxHealth;
            gameController.currentHealth += 25;
            gameController.UpdateHealingBar();

            audioSource.Play();
            hasPickedup = true;

            // Flash message
            string messageToSend = "Max health increased by 25!";
            DisplayMessage.MessageToQueue(messageToSend);
        }
    }
}
