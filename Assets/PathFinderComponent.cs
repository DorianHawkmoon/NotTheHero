using System.Collections.Generic;
using UnityEngine;

public class PathFinderComponent : MonoBehaviour {

    public List<Node> aPath;

    private TargetComponent target;
    private Grid grid;

    public void Awake() {
        grid = FindObjectOfType<Grid>();
    }

    // Use this for initialization
    void Start () {
        target = GetComponent<TargetComponent>();	
	}
	
	void Update () {
        Vector3 targetPosition = target.GetTargetPosition();

        FindPath(transform.position, targetPosition);
	}

    private void FindPath(Vector3 startPositon, Vector3 targetPosition) {
        Node startNode = grid.GetNodeFromWorldPoint(startPositon);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; ++i) {
                if(openSet[i].fCost < currentNode.fCost 
                    || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode)) {
                if(!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node targetNode) {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        aPath = path;
    }

    private int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        } else {
            return 14 * dstX + 10 * (dstY - dstX);
        }

    }
}
