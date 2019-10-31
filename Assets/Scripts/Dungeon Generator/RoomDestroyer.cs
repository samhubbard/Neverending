using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDestroyer : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ClosedRoom") == true)
        {
            // Destroy the game object (this prevents the room blocker from dropping on the starting room
            Destroy(collision.gameObject);
        }
    }
}
