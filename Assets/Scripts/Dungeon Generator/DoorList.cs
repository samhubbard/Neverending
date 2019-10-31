using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorList : MonoBehaviour {

    RoomEvent roomEvent;

	void Start () {
		// adds the door that this script is attached to to the room event for future
		// destruction
        // roomEvent = GameObject.FindGameObjectWithTag("RoomEvent").GetComponent<RoomEvent>();
        // roomEvent.listOfDoors.Add(this.gameObject);
    }
}
