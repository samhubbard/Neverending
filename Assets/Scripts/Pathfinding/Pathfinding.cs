using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    // defining the grid
    PFGrid grid;
    PathRequestManager requestManager;

    public Node playerNode;

    // temp
    public List<Node> displayPathing = new List<Node>();

    private void Awake()
    {
        // linking the grid to the grid script
        grid = GetComponent<PFGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    /* A* Psuedo code:
     * 
     * Open set of nodes to be evaluated
     * Closed set of nodes that have already been evaluated
     * 
     * loop
     *  current = node in the open set with the lowest fCost
     *  remove curent node from the open set
     *  add the current node to the closed set
     * 
     * if the current node is the target node
     *  return <-- this is where the target node has been found
     * 
     * foreach neighbor of the current node
     *  if neighbor is not traversable or neighbor is in the closed set
     *      skip to the next neighbor
     * 
     *  if the new path to the neighbor is shorter or the neighbor is not in the open set
     *      set the fCost of the neighbor
     *      set the parent of the neighbor to the current node
     *      if the neighbor node is not in the open set
     *          add the neighbor to the open set
     */

    // initiate the A* pathfinder co-routine
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // get the node locations for the starting position and the target position
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //print("X: " + startPos.x + ", Y: " + startPos.y);
        //print("[" + targetNode.gridX + ", " + targetNode.gridY + "]");

        playerNode = targetNode;

        // setup the sets of open and closed nodes
        //List<Node> openGridPoints = new List<Node>();
        Heap<Node> openGridPoints = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedGridPoints = new HashSet<Node>();

        // add the current node to the open set
        openGridPoints.AddNewT(startNode);

        // start the loop
        while (openGridPoints.Count > 0) {
            // set the current node to the first item in the open set
            Node currentNode = openGridPoints.RemoveFirst();
            closedGridPoints.Add(currentNode);

            // if the current node is the target node, the path is complete
            // retrace the path and exit out of the algorithm
            if (currentNode == targetNode) {
                pathSuccess = true;
                break;
            }

            // checking all of the neighbors of the current node
            foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
                // if the neighbor is not usable, skip it
                if (!neighbor.walkable || closedGridPoints.Contains(neighbor)) {
                    continue;
                }

                // set up the new movement cost to the neighbor
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // if the distance is shorter to the neighbor, or the neighbor is not in the open set
                if (newMovementCostToNeighbor < neighbor.gCost || !openGridPoints.Contains(neighbor)) {
                    // set the fCost of the neighbor
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);

                    // make parent of the neighbor the current node
                    neighbor.parent = currentNode;

                    // if the neighbor is not a part of the open set, add it
                    if (!openGridPoints.Contains(neighbor)) {
                        openGridPoints.AddNewT(neighbor);
                    } else {
                        openGridPoints.UpdateItem(neighbor);
                    }
                }
            } 
        }

        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode); // set up the waypoints to the path
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess); // send the success over to the manager

    }

    // retracing the path and creating a list of nodes that make up the path
    Vector3[] RetracePath(Node startNode, Node endNode) {
        // instantiate the list and set the current node to the end node
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // while the current node is not at the start node
        while (currentNode != startNode) {
            // add the current node to the list and move to the node's parent
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);

        // once the loop has completed (and the path is complete)
        // reveruse the list so that the list is in order from the start node to the end
        Array.Reverse(waypoints);

        displayPathing = path;
        return waypoints;

    }

    Vector3[] SimplifyPath (List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPosition);
            }

            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    // the method to calculate the distance between two nodes
    int GetDistance(Node nodeA, Node nodeB) {
        // getting the x and y distances to whole numbers
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // return the distance based on whether x or y is larger
        return distanceX > distanceY ? 
            14 * distanceY + 10 * (distanceX - distanceY) 
                : 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
