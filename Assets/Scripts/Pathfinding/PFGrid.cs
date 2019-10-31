using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFGrid : MonoBehaviour {

    // variables that can be changed in the inspector for easy editing
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;

    public bool displayGridGizmos;

    // the 2d array that will hold the grid
    public Node[,] grid;

    // temp
    Pathfinding pather;
    List<Node> pathing = new List<Node>();

    // numbers needed for grid generation calculations
    float nodeDiameter;
    public int gridSizeX, gridSizeY;

    private void Awake()
    {


        // creating the grid
        CreateGrid();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid() {
        //temp
        pather = GetComponent<Pathfinding>();

        // setting the diameter value and then finding how many elements will be in the array
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        // Instantiate the array and set the sizes for the two different dimensions
        grid = new Node[gridSizeX, gridSizeY];

        // Find the bottom left most portion to start populating the grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y /2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // method for finding the world position of a particular object
    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        int x = Mathf.RoundToInt((worldPosition.x + gridWorldSize.x / 2 - nodeRadius) / nodeDiameter);
        int y = Mathf.RoundToInt((worldPosition.y + gridWorldSize.y / 2 - nodeRadius) / nodeDiameter);
        
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        return grid[x, y];
    }

    // generating a list for all of the neighbors surrounding the selected node
    public List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        // running nested loops for the x and y axis
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }

                // setting the grid coordinates
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // ensure that the values are not outside of the realm of possibility
                // and add it to the list
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors; // return the list of neighbors
    }

    // temporary for visual reference and verification
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }

            pathing = pather.displayPathing;
            foreach (Node n in pathing) {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }

            if (pather.playerNode != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(pather.playerNode.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        } 
    }
}
