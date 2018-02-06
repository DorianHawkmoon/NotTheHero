using System.Collections;
using System;
using UnityEngine;

public class MovementComponent : MonoBehaviour {
    public float velocity = 1;

    //private CharacterController controller;
    private PathFinderComponent pathFinder;
    private Vector3[] path;
    private int targetIndex;

    // Use this for initialization
    void Start () {
        //controller = GetComponent<CharacterController>();
        pathFinder = GetComponent<PathFinderComponent>();
        pathFinder.RegisterOnPathChange(OnChangedPath);
    }
	
	// Update is called once per frame
	private void OnChangedPath() {
        //get new path
        path = pathFinder.GetPath();
        if (path != null && path.Length > 0) {
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                ++targetIndex;
                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            
            /*currentWaypoint.z = 0;
            Vector3 origin = transform.position;
            origin.z = 0;
            Vector3 move = currentWaypoint - origin;

            controller.Move(move * Time.deltaTime * velocity);
            Error: not working because of the comparison transform.position == currentWayPoint
             */
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, velocity * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; ++i) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
