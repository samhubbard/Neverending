using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    // Get a reference to the player's location
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // drop the inventory item onto the ground
    public void SpawnDroppedItem()
    {
        Vector2 dropPosition = new Vector2(player.position.x, player.position.y + 3);
        Instantiate(item, dropPosition, Quaternion.identity);
    }
}
