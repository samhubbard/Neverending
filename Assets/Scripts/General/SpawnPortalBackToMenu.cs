using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortalBackToMenu : MonoBehaviour
{
    public GameObject portal;

    public void Spawn()
    {
        Instantiate(portal, transform.position, Quaternion.identity);
    }
}
