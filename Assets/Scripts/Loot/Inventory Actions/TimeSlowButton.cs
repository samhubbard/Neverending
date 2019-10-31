using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowButton : MonoBehaviour
{
    public GameObject timeSlowObject;
    public GameObject soundEffect;

    // Ensure that the room event is active
    // instantiate the slow object into the scene at the player location
    // notify the player and then destroy the object in the inventory
    public void SlowAllEnemies()
    {
        if (RoomEvent.roomEventActive || StartBossFight.bossRoomEventActive)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            // Figure out how to slow everything enemy releated down
            Instantiate(timeSlowObject, player.position, Quaternion.identity);
            Instantiate(soundEffect, player.position, Quaternion.identity);

            // Flash message
            string messageToSend = "Enemies and their abilities slowed!";
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