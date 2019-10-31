using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDoor : MonoBehaviour {

    public GameObject door;
    public RoomEvent roomEvent;
    Transform room;
    bool doorSpawned = false;

	// Use this for initialization
	void Start () {
        // get the transform for the room
        room = this.transform.parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
        // spawn the doors in
        if (roomEvent.hasActivated && !doorSpawned) {
            SpawnDoorIn();
        }
	}

    public void SpawnDoorIn() {
        // instantiate the door sprite in and place it in the scene
        // then add it to the list of doors so that it can easily be destroyed later
        GameObject spawnedDoor = Instantiate(door, transform.position, Quaternion.identity, room);
        //roomEvent.listOfDoors.Add(spawnedDoor);
        doorSpawned = true;
    }
}
