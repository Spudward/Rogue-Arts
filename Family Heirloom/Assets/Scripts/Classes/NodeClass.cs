using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClass
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX, gridY;
    public NodeClass parent;

    public int gCost;
    public int hCost;


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
}
