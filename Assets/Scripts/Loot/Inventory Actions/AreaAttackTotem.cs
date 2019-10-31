using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackTotem : MonoBehaviour
{
    public GameObject AOETotem;
    public GameObject soundObject;

    // Ensure that the room event is active
    // instantiate the AOE totem into the scene at the player location
    // notify the player and then destroy the object in the inventory
    public void ActivateAbility()
    {
        if (RoomEvent.roomEventActive || StartBossFight.bossRoomEventActive)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            // this is where the area attack totem logic goes
            Instantiate(AOETotem, player.position, Quaternion.identity);
            Instantiate(soundObject, player.position, Quaternion.identity);

            // flash message
            string messageToSend = "Area Attack Totem Deployed!";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject);
        }
        else
        {
            string messageToSend = "No enemies...";
            DisplayMessage.MessageToQueue(messageToSend);
        }
        
    }
}
