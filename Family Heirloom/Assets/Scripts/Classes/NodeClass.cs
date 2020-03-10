using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClass : IHeapItem<NodeClass>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX, gridY;
    public NodeClass parent;

    public int gCost;
    public int hCost;
    int heapIndex;


    public NodeClass (bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

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

    public int CompareTo (NodeClass nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
