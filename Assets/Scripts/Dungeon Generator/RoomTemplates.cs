using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {

    // room arrays based on their openings
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    // the room blocker
    public GameObject closedRoom;

    // list of rooms
    public List<GameObject> rooms;

    // the variables needed to spawn the exit sprite
    public float waitTime;
    private bool spawnedExit;
    public GameObject exitSprite;
    public GameObject exitSpriteToBoss;

    private void Update()
    {
        // wait until the wait time is up to ensure that the level is finished generating
        if (waitTime <= 0 && !spawnedExit) {
            // instantiate the exit sprite into the last room and set the bool to true
            // so that it doesn't spawn multiple exits
            Vector3 offset;

            // this switch statement determines where to place the exit portal
            switch (rooms[rooms.Count - 1].name)
            {
                case "Right(Clone)":
                case "Left(Clone)":
                case "Bottom(Clone)":
                case "LeftRight(Clone)":
                    offset = new Vector3(0f, 14f, 0f);
                    break;

                case "RightBottom(Clone)":
                    offset = new Vector3(0f, 12f, 0f);
                    break;

                case "TopBottom(Clone)":
                case "TopRight(Clone)":
                    offset = new Vector3(-13f, 0f, 0f);
                    break;

                case "TopLeft(Clone)":
                    offset = new Vector3(13f, 0f, 0f);
                    break;

                case "Top(Clone)":
                    offset = new Vector3(0f, -13f, 0f);
                    break;

                default:
                    offset = new Vector3(0f, 0f, 0f);
                    break;
            }

            // instantiate the portal in the last room that was created at the offset
            // making the room the parent object
            GameController c = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            if (c.currentLevel == 3)
            {
                GameObject exit = Instantiate(exitSpriteToBoss, 
                rooms[rooms.Count - 1].transform.position + offset, 
                Quaternion.identity, 
                rooms[rooms.Count - 1].transform);
            }
            else
            {
                GameObject exit = Instantiate(exitSprite, 
                rooms[rooms.Count - 1].transform.position + offset, 
                Quaternion.identity, 
                rooms[rooms.Count - 1].transform);
            }
            
            // setting the boolean to true so that it doesn't continue to try to spawn in exit rooms
            spawnedExit = true;
        } else {
            // if the wait time hasn't been met yet, subtract it from the elapsed time
            waitTime -= Time.deltaTime;
        }
    }
}
