using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBCreate : MonoBehaviour {

    private DBAccess database;

    // Use this for initialization
    void Start ()
    {
        database = GetComponent<DBAccess>();
        database.OnApplicationStart();
	}
}
