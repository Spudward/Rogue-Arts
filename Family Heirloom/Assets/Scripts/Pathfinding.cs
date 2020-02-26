using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AIHiveMind grid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<AIHiveMind>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        NodeClass startNode = grid.NodeFromWorldPoint(startPos);
        NodeClass targetNode = grid.NodeFromWorldPoint(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Heap<NodeClass> openSet = new Heap<NodeClass>(grid.MaxSize);
        HashSet<NodeClass> closedSet = new HashSet<NodeClass>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            NodeClass currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                //sw.Stop();
                //print("path found: " + sw.ElapsedMilliseconds + " ms");
                pathSuccess = true;
                break;
            }

            foreach (NodeClass neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }

        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); 
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath (NodeClass startNode, NodeClass endNode)
    {
        List<NodeClass> path = new List<NodeClass>();
        NodeClass currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        //Vector3[] waypoints = path.ToArray();

        waypoints.Reverse();



        grid.path = path;

        return waypoints;
    }

    Vector3[] SimplifyPath (List<NodeClass> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }

            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(NodeClass nodeA, NodeClass nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        } else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
