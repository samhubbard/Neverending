using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour {

    // This script just adds the room to an array... I may just move this into a pre-existing script in 
    // the room prefab. But it isn't hurting anything like this.

    RoomTemplates templates;

    private void Start()
    {
        templates = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
    }
}
