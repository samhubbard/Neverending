using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTotem : MonoBehaviour
{
    public GameObject healingTotem;
    public GameObject soundEffect;

    // Ensure that the room event is active
    // instantiate the totem into the scene at the player location
    // notify the player and then destroy the object in the inventory
    public void ActivateAbility()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // this is where the logic will go to drop the healing totem
        Instantiate(healingTotem, player.position, Quaternion.identity);
        Instantiate(soundEffect, player.position, Quaternion.identity);

        // flash message
        string messageToSend = "Healing Totem Dropped!";
        DisplayMessage.MessageToQueue(messageToSend);

        Destroy(gameObject);
    }
}
