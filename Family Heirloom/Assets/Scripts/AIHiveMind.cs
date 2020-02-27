using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHiveMind : MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    public Transform target;
    public LayerMask unwalkable;
    public Vector2 gridSize;
    public float nodeRadius;
    NodeClass[,] grid;

    float nodeDiameter;
    int gridSizex, gridSizey;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizex = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizey = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizex * gridSizey;
        }
    }
    void CreateGrid()
    {
        grid = new NodeClass[gridSizex, gridSizey];
        Vector3 worldBottomLeft = transform.position- Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (int x = 0; x < gridSizex; x++)
        {
            for (int y = 0; y < gridSizey; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkable));
                grid[x, y] = new NodeClass(walkable, worldPoint, x, y);
            }
        }
    }

    public List<NodeClass> GetNeighbours(NodeClass node)
    {
        List<NodeClass> neighbours = new List<NodeClass>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizex && checkY >= 0 && checkY < gridSizey)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public List<NodeClass> path;
    public NodeClass NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPosition.z + gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizex - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizey - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (NodeClass n in path)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));

                }
            }
        }
        else
        {

            if (grid != null)
            {
                NodeClass targetNode = NodeFromWorldPoint(target.position);
                foreach (NodeClass node in grid)
                {
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;
                    if (path != null)
                    {
                        if (path.Contains(node))
                        {
                            Gizmos.color = Color.green;
                        }
                    }
                    if (targetNode == node)
                    {
                        Gizmos.color = Color.blue;
                    }

                    Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
    }
}
