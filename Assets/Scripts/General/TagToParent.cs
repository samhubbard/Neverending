using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagToParent : MonoBehaviour {

    public Transform parent;

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position;
    }
}