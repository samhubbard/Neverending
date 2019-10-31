using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterHealthPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    private bool canDestroy;
    private bool hasDestroyed;
    private bool hasPickedup;
    private float timer;

    // sets up the timer and all of the booleans
    void Start()
    {
        canDestroy = false;
        hasDestroyed = false;
        hasPickedup = false;
        timer = 0.5f;
    }

    // keeps tabs on the timer and eventually will delete the item from the scene
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

            if (controller.currentHealth < GameController.maxHealth)
            {
                AudioSource audioSource = gameObject.transform.parent.gameObject.AddComponent<AudioSource>();
                audioSource.clip = pickupSound;
                audioSource.volume = 1.0f * GameController.sfxVolume;

                int healAmount = 60;
                PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


                player.TakeHealing(healAmount);

                audioSource.Play();
                hasPickedup = true;

                // Flash message
                string messageToSend = "Greater healed by " + Mathf.Abs(healAmount) + "!";
                DisplayMessage.MessageToQueue(messageToSend);
            }
            else
            {
                string messageToSend = "No need for healing.";
                DisplayMessage.MessageToQueue(messageToSend);
            }
        }
    }
}
