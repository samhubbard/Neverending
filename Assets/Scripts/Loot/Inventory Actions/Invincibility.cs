using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public GameObject invincibleBuff;
    public GameObject soundEffect;

    // Ensure that the room event is active
    // instantiate the invincibility object into the scene at the player location
    // notify the player and then destroy the object in the inventory
    public void ActivateInvincibility()
    {
        if (RoomEvent.roomEventActive || StartBossFight.bossRoomEventActive)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            // invincibility logic goes here
            Instantiate(invincibleBuff, player.position, Quaternion.identity, player);
            Instantiate(soundEffect, player.position, Quaternion.identity);

            // Flash message
            string messageToSend = "Invincibility Active!";
            DisplayMessage.MessageToQueue(messageToSend);


            Destroy(gameObject);
        }
        else
        {
            string messageToSend = "No enemies in the area.";
            DisplayMessage.MessageToQueue(messageToSend);
        }
    }
}
