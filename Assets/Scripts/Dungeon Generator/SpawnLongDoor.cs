using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLongDoor : MonoBehaviour {

    public GameObject door;
    public RoomEvent roomEvent;
    Transform room;
    bool doorsSpawned = false;

	// Use this for initialization
	void Start () {
        // set the transform for the current room
        room = this.transform.parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
        // if the room event has been activated, spawn the door in
        if (roomEvent.hasActivated && !doorsSpawned) {
            SpawnDoorIn();
        }
	}

    public void SpawnDoorIn() {
        // instantiate the door into the scene and add it to the list of doors
        GameObject spawnedDoor = Instantiate(door, transform.position, Quaternion.identity, room);
        //roomEvent.listOfDoors.Add(spawnedDoor);
        doorsSpawned = true;
    }
}
