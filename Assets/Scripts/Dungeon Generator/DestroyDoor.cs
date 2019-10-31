using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDoor : MonoBehaviour {

	// simply used to destroy the doors

    void OpenUpRoom() {
        Destroy(this.gameObject);
    }
}
