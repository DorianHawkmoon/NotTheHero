using UnityEngine;

/// <summary>
/// Node class for the A* pathfinding algorithm
/// Implements heap interface
/// </summary>
public class Node : IHeapItem<Node>  {
    /// <summary>
    /// The node is walkable
    /// </summary>
    public bool walkable;
    /// <summary>
    /// World position for the node
    /// </summary>
    public Vector3 worldPosition;
    /// <summary>
    /// Parent of the node
    /// </summary>
    public Node parent;

    /// <summary>
    /// X position in the grid
    /// </summary>
    public int gridX;
    /// <summary>
    /// Y position in the grid
    /// </summary>
    public int gridY;

    /// <summary>
    /// Distance/cost from starting node
    /// </summary>
    public int gCost;
    /// <summary>
    /// Heuristic cost/distance from end node
    /// </summary>
    public int hCost;

    /// <summary>
    /// Index in heap structure
    /// </summary>
    private int heapIndex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="walkable">If is walkable</param>
    /// <param name="worldPosition">world position</param>
    /// <param name="gridX">x position in grid</param>
    /// <param name="gridY">y position in grid</param>
    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY) {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    /// <summary>
    /// get final/total cost for the path
    /// </summary>
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    /// <summary>
    /// Get and set for the index of heap
    /// </summary>
    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    /// <summary>
    /// Compare two nodes, first for total cost and after for heuristic cost
    /// </summary>
    /// <param name="nodeToCompare"></param>
    /// <returns></returns>
    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}
