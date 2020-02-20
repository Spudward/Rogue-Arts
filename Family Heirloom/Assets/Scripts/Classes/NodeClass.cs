using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClass
{
    public bool walkable;
    public Vector3 worldPos;
    public NodeClass (bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPos = _worldPos;
    }
}
