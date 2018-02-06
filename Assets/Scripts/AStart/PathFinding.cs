﻿using UnityEngine;using System.Collections;using System.Collections.Generic;using System;using System.Diagnostics;public class PathFinding : MonoBehaviour {      private TargetComponent target;    PathRequestManager requestManager;    private Grid grid;    public void Awake() {        requestManager = GetComponent<PathRequestManager>();        grid = GetComponent<Grid>();    }    // Use this for initialization    void Start() {        target = GetComponent<TargetComponent>();    }    void Update() {        if (Input.GetButtonDown("Jump")) {            Vector3 targetPosition = target.GetTargetPosition();            FindPath(transform.position, targetPosition);        }    }        public void StartFindPath(Vector3 startPosition, Vector3 targetPosition) {
        StartCoroutine(FindPath(startPosition, targetPosition));    }    private IEnumerator FindPath(Vector3 startPositon, Vector3 targetPosition) {        Stopwatch sw = new Stopwatch();        sw.Start();        Vector3[] waypoints = new Vector3[0];        bool pathSuccess=false;        Node startNode = grid.GetNodeFromWorldPoint(startPositon);        Node targetNode = grid.GetNodeFromWorldPoint(targetPosition);        if (startNode.walkable && targetNode.walkable) {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");
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
            }        }        yield return null;        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
        }        requestManager.FinishedProcessingPath(waypoints,pathSuccess);    }    private Vector3[] RetracePath(Node startNode, Node targetNode) {        List<Node> path = new List<Node>();        Node currentNode = targetNode;        while (currentNode != startNode) {            path.Add(currentNode);            currentNode = currentNode.parent;        }        Vector3[] waypoints = SimplifyPath(path);        Array.Reverse(waypoints);        return waypoints;    }    private Vector3[] SimplifyPath(List<Node> path) {
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
    }    private int GetDistance(Node nodeA, Node nodeB) {        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);        if (dstX > dstY) {            return 14 * dstY + 10 * (dstX - dstY);        } else {            return 14 * dstX + 10 * (dstY - dstX);        }    }}