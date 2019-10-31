using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHealingTotem : MonoBehaviour
{
    // timer variables
    public float totalTimeTotemActive = 5.0f;
    private float abilityTimer;

    public float timeBetweenHeals = 0.5f;
    private float healingTimer;

    private bool canReceiveHeals = false;

    AudioSource audioSource;

    // setup the timers and get a reference to the audio source
    void Start()
    {
        abilityTimer = totalTimeTotemActive;
        healingTimer = timeBetweenHeals;

        audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
    }

    // Check the timers, heal the player if they are in the radius, and destroy the object when the time is up
    void Update()
    {
        if (abilityTimer <= 0)
        {
            // Time's up!

            string messageToSend = "Healing Totem Expired.";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject.transform.parent.gameObject, 0.5f);
            Destroy(gameObject, 0.5f);
            
        }
        else
        {
            abilityTimer -= Time.deltaTime;
        }

        if (healingTimer <= 0)
        {
            if (canReceiveHeals)
            {
                GameController control = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

                if (control.currentHealth < GameController.maxHealth)
                {
                    PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                    player.TakeHealing(10);
                    audioSource.volume = audioSource.volume * GameController.sfxVolume;
                    audioSource.Play();
                }
                
            }
            healingTimer = timeBetweenHeals;
        }
        else
        {
            healingTimer -= Time.deltaTime;
        }
    }

    // when the player enters the area, set it up so that they can receive heals
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canReceiveHeals = true;
        }
    }

    // when the player leaves the area, turn off healing
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canReceiveHeals = false;
        }
    }
}
