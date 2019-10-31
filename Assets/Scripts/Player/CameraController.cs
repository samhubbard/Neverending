using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // grab the player and set the offset
    Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -30f);
    private Vector3 velocity = Vector3.zero;
    public float dampTime = 3.0f;
    private bool followingPlayer = false;
    public static bool transitioning = false;
    private float cameraSize;
    private bool mapOpen;
    private Camera cam;

    // links to the player and sets up for zooming, and following
    private void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        followingPlayer = true;
        cam = GetComponent<Camera>();
        cameraSize = 10;
    }

    // checks up on the status of the camera and reacts to the different flags that are sent in from outside scripts
    void Update()
    {
        if (target != null && !transitioning)
        {
            if (followingPlayer)
            {
                transform.position = target.position + offset;
            }
        }
        else if (target != null && transitioning)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, dampTime);
        }

        if (mapOpen && cam.orthographicSize != cameraSize)
        {
            cam.orthographicSize = Mathf.SmoothStep(cam.orthographicSize, cameraSize, .25f);
        }
        else if (!mapOpen && cam.orthographicSize != cameraSize)
        {
            cam.orthographicSize = Mathf.SmoothStep(cam.orthographicSize, cameraSize, .25f);
        }
    }

    // stops following the player and sets the target to the boss
    public void SetToBoss()
    {
        followingPlayer = false;
        target = GameObject.FindWithTag("Boss").GetComponent<Transform>();
    }

    // sets the target to the player and starts following the player again
    public void SetBackToPlayer()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        followingPlayer = true;
    }

    public void SetToPortalSpawnPoint()
    {
        target = GameObject.FindWithTag("PortalSpawnPoint").GetComponent<Transform>();
    }

    // sets the camera size to 50... essentially zooming the camera way out
    public void OpenMap()
    {
        cameraSize = 50;
        mapOpen = true;
    }

    // brings the camera back down to the player
    public void CloseMap()
    {
        cameraSize = 10;
        mapOpen = false;
    }
}
