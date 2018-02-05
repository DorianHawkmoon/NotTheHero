using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderComponent : MonoBehaviour {

    private TargetComponent target;
    private Grid grid;

    public void Awake() {
        grid = FindObjectOfType<Grid>();
    }

    // Use this for initialization
    void Start () {
        target = GetComponent<TargetComponent>();	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = target.GetTargetPosition();
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
        }
    }
}
