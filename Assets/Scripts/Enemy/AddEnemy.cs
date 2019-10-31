using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemy : MonoBehaviour {

    RoomEvent roomEvent;

	// Use this for initialization
	void Start () {
		// adding an enemy to the list of enemies
		// the list is what will indicate if the room event is over or not
        // roomEvent = GameObject.FindGameObjectWithTag("RoomEvent").GetComponent<RoomEvent>();
        // roomEvent.listOfEnemies.Add(this.gameObject);
	}
}
