using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTotem : MonoBehaviour
{
    public GameObject slowingTotem;
    public GameObject soundEffect;

    // Ensure that the room event is active
    // instantiate the totem into the scene at the player location
    // notify the player and then destroy the object in the inventory
    public void ActivateAbility()
    {
        if (RoomEvent.roomEventActive || StartBossFight.bossRoomEventActive)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            // this is where the logic will go for the slowing totem
            Instantiate(slowingTotem, player.position, Quaternion.identity);
            Instantiate(soundEffect, player.position, Quaternion.identity);

            // flash message
            string messageToSend = "Slowing Totem Deployed!";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject);
        }
        else
        {
            string messageToSend = "No enemies present.";
            DisplayMessage.MessageToQueue(messageToSend);
        }
    }
}
