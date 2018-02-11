using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Class responsible for finding paths given start and end points
/// </summary>
public class PathFinding : MonoBehaviour {
    /// <summary>
    /// Requester where petitions are stored
    /// </summary>
    PathRequestManager requestManager;
    private GridLevel grid;

    /// <summary>
    /// Get the requester and grid of A*
    /// </summary>
    public void Awake() {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<GridLevel>();
    }
    
    /// <summary>
    /// Start finding a specific path
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="targetPosition"></param>
    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition) {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    /// <summary>
    /// Find a path
    /// </summary>
    /// <param name="startPositon"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private IEnumerator FindPath(Vector3 startPositon, Vector3 targetPosition) {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess=false;

        Node startNode = grid.GetNodeFromWorldPoint(startPositon);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPosition);

        //if (startNode.walkable && targetNode.walkable) {
        if (targetNode.walkable) {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    //sw.Stop();
                    //print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) {
                            openSet.Add(neighbour);
                        } else {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints,pathSuccess);

    }

    /// <summary>
    /// After setting parents of nodes looking for a path
    /// do reverse way and store the array of nodes of the path in correct order
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    private Vector3[] RetracePath(Node startNode, Node targetNode) {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    /// <summary>
    /// Simplify the path giving only the needed points to follow the path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; ++i) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld) {
                waypoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    /// <summary>
    /// Distance between nodes
    /// Magic numbers comes from
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    private int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        } else {
            return 14 * dstX + 10 * (dstY - dstX);
        }

    }

}
