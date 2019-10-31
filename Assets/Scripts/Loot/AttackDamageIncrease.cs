using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageIncrease : MonoBehaviour
{
    public AudioClip soundEffect;
    private bool canDestroy;
    private bool hasDestroyed;
    private bool hasPickedup;
    private float timer;

    // sets up all of the booleans for the sequence of events
    void Start()
    {
        canDestroy = false;
        hasDestroyed = false;
        hasPickedup = false;
        timer = 0.5f;
    }

    // checks the timers, sets destroy flags, and eventually destroys the object from the scene
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
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            //set it up for storage... so that the GameController can handle scene changes
            gameController.playerStats.AttackDamageIncreaseAmount += 4;
            player.bulletDamage += 4;
            gameController.UpdatePlayerStats();

            audioSource.Play();
            hasPickedup = true;

            // Flash message
            string messageToSend = "Bullet damage increased!";
            DisplayMessage.MessageToQueue(messageToSend);
        }
    }
}
