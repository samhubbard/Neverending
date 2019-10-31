using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    // member variables
    public bool walkable;
    public Vector3 worldPosition;
    public int gCost, hCost, gridX, gridY;
    public Node parent;
    int heapIndex;

    // public constructor
    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY) {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    // calculating the fCost
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    // bringing in the ability to get and set the heap index
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    // Comparing the two different nodes and returning the inverse
    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        // returning the inverse because the lower value is higher on the list of
        // priority
        return -compare;
    }
}
