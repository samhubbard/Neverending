using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityActive : MonoBehaviour
{
    // timer variables
    public float totalInvincibilityTime = 5.0f;
    private float timer;

    // Set the player to invincible and setup the timer
    void Start()
    {
        PlayerController.invincible = true;
        timer = totalInvincibilityTime;
    }

    // Once the timer has run up, turn off invincibility
    void Update()
    {
        if (timer <= 0)
        {
            // time's up!
            PlayerController.invincible = false;

            string messageToSend = "Invincibility has run out.";
            DisplayMessage.MessageToQueue(messageToSend);
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
