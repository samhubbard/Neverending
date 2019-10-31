using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

    // required variables for room spawning
    public int openingDirection;
    // 1 -> need bottom door
    // 2 -> need top door
    // 3 -> need left door
    // 4 -> need right door

    private RoomTemplates templates; // bringing in all of the rooms
    private int random; 
    public bool spawned = false;

    public float waitTime = 4f;

    private void Start()
    {
        // Destroy the spawn point so that the scene isn't getting cluttered with them
        Destroy(gameObject, waitTime);

        // Set up where the room templates are and invoke the function to start spawning rooms
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        if (!spawned) {

            // depending on the opening direction needed, get a random number for the array index and instantiate the room
            if (openingDirection == 1)
            {
                // Need bottom door
                random = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[random], transform.position, templates.bottomRooms[random].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                // Need top door
                random = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[random], transform.position, templates.topRooms[random].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                // Need left door
                random = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[random], transform.position, templates.leftRooms[random].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                // Need right door
                random = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[random], transform.position, templates.rightRooms[random].transform.rotation);
            }

            spawned = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if there will be some openings to the great infinity, place a room blocker
        if (collision.CompareTag("SpawnPoint")) {
            if (collision.GetComponent<RoomSpawner>() != null)
            {
                if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
                {
                    // spawn in a blocker room
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            spawned = true;
        }
    }
}
