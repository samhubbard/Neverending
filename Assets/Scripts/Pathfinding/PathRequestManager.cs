using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>(); // setting up the queue for the pathfinder
    PathRequest currentPathRequest; // setting up the current path request

    static PathRequestManager instance; // setting the instance of the manager
    Pathfinding pathfinding; // linking the pathfinding script to this

    bool isProcessingPath; // a boolean that indicates if a path is being processed

    private void Awake()
    {
        instance = this; // set the instance to the current iteration
        pathfinding = GetComponent<Pathfinding>(); // link the pathfinding script to here
    }


    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback); // set the new request up
        instance.pathRequestQueue.Enqueue(newRequest); // put the path into the queue
        instance.TryProcessNext(); // attempt to process the next in the queue
    }

    void TryProcessNext() {
        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            currentPathRequest = pathRequestQueue.Dequeue(); // remove the requested path from the queue
            isProcessingPath = true; // tell the script that there is a path being requested
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // find the path
        }
    }

    // once the path has been found
    public void FinishedProcessingPath(Vector3[] path, bool success) {
        currentPathRequest.callback(path, success); // setup the callback
        isProcessingPath = false; // tell the script that there is no longer a path being calculated
        TryProcessNext(); // try to process the next in the queue
    }

    struct PathRequest {
        // member variables
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        // public constructor
        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback) {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callback;
        }
    }
}
