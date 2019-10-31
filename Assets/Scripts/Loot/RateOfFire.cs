using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateOfFire : MonoBehaviour
{
    public AudioClip soundEffect;
    private bool canDestroy;
    private bool hasDestroyed;
    private bool hasPickedup;
    private float timer;

    // sets up the timer and flags for function
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
            GameController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

            if (!controller.playerStats.MaxRateOfFireReached)
            {
                AudioSource audioSource = gameObject.transform.parent.gameObject.AddComponent<AudioSource>();
                audioSource.clip = soundEffect;
                audioSource.volume = 1.0f * GameController.sfxVolume;

                PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                player.firingSpeed = player.firingSpeed / 1.25f;
                
                controller.playerStats.TotalRateOfFire = player.firingSpeed;
                controller.playerStats.TotalRateOfFireIncreases += 1;
                if (controller.playerStats.TotalRateOfFireIncreases >= 4)
                {
                    controller.playerStats.MaxRateOfFireReached = true;
                }

                controller.UpdatePlayerStats();

                audioSource.Play();
                hasPickedup = true;

                // Flash message
                string messageToSend = "Rate of Fire Increased!";
                DisplayMessage.MessageToQueue(messageToSend);
            }
        }
    }
}
