using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReflection : MonoBehaviour
{
    public GameObject damageReflectionObject;
    public GameObject soundEffect;

    // Ensure that the room event is active
    // instantiate the reflection object into the scene at the player location
    // notify the player and then destroy the object in the inventory
    public void ActivateAbility()
    {
        if (RoomEvent.roomEventActive || StartBossFight.bossRoomEventActive)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            // this is where the logic goes to use damage reflection
            Instantiate(damageReflectionObject, player.position, Quaternion.identity);
            Instantiate(soundEffect, player.position, Quaternion.identity);

            // flash message
            string messageToSend = "Damage Reflection Active!";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject);
        }
        else
        {
            string messageToSend = "No active enemies...";
            DisplayMessage.MessageToQueue(messageToSend);
        }
    }
}
