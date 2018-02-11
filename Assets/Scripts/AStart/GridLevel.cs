using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grid of the level to be used by A* algorithm
/// </summary>
public class GridLevel  : MonoBehaviour {
    /// <summary>
    /// If we show gizmos
    /// </summary>
    public bool showGrid=false;
    /// <summary>
    /// The layer that determines the unwalkable nodes
    /// </summary>
    public LayerMask unwalkableMask;
    /// <summary>
    /// Size of the world grid
    /// </summary>
    public Vector2 gridWorldSize;
    /// <summary>
    /// Size of a node
    /// </summary>
    public float nodeRadius;
    /// <summary>
    /// Grid
    /// </summary>
    private Node[,] grid;
    /// <summary>
    /// diameter of the node
    /// </summary>
    private float nodeDiameter;
    /// <summary>
    /// size of grid in number of nodes
    /// </summary>
    private int gridSizeX, gridSizeY;

    /// <summary>
    /// Getter of node diameter
    /// </summary>
    public float NodeDiameter {
        get {
            return nodeDiameter;
        }
    }

    /// <summary>
    /// Create the grid
    /// </summary>
    public void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    /// <summary>
    /// Get number of nodes
    /// </summary>
    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    /// <summary>
    /// Create the grid
    /// </summary>
    private void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 -Vector3.up*gridWorldSize.y/2;

        for(int x=0; x< gridSizeX; ++x) {
            for (int y = 0; y < gridSizeY; ++y) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
                                        + Vector3.up * (y*nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

            }
        }
    }

    /// <summary>
    /// Get neighbours of a given node
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; ++x) {
            for (int y = -1; y <= 1; ++y) {
                if(x==0 && y == 0) {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >=0 && checkX < gridSizeX 
                    && checkY >=0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    /// <summary>
    /// Get a node given world position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Node GetNodeFromWorldPoint(Vector3 position) {
        float percentX = (position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (position.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    /// <summary>
    /// Draw grid
    /// </summary>
    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0.5f));

        if (grid != null && showGrid) {
            foreach (Node node in grid) {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.05f));
            }
        }
    }
}
