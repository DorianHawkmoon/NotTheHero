//#define DEBUG_MovementComponent

using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour {
#if DEBUG_MovementComponent
    private static DebugLog log = new DebugLog("MovementComponent");
#endif

    /// <summary>
    /// Velocity of the game object
    /// </summary>
    public float velocity = 1;
    /// <summary>
    /// Can the gameobject move?
    /// </summary>
    private bool canMove = true;
    /// <summary>
    /// Path to follow
    /// </summary>
    private Vector3[] path;
    /// <summary>
    /// Which index of the path we are targeting
    /// </summary>
    private int targetIndex;
    /// <summary>
    /// The direction of movement to achieve the next point of the path
    /// </summary>
    private Vector3 moveDirection;
    /// <summary>
    /// Are we moving? used to tell the animator
    /// </summary>
    private bool moving; //TODO check if needed, posible bug if we stop movement, but the variable still on true (bad animation then)

    /// <summary>
    /// Components needed
    /// </summary>
    private PathFinderComponent pathFinder;
    private Animator animator;

    /// <summary>
    /// Get needed components
    /// </summary>
    public void Start() {
        animator = GetComponent<Animator>();
        pathFinder = GetComponent<PathFinderComponent>();
        Debug.Assert(pathFinder != null);
        pathFinder.RegisterOnPathChange(OnChangedPath);
    }

    /// <summary>
    /// Stop moving
    /// </summary>
    public void StopMovement() {
        SetCanMove(false);
    }

    /// <summary>
    /// Start moving thought a target
    /// </summary>
    public void MoveToTarget() {
        SetCanMove(true);
    }

    /// <summary>
    /// We can move, but don't start moving to the target
    /// </summary>
    public void CanMove() {
        canMove = true;
    }

    /// <summary>
    /// Stop or start a new path movement
    /// </summary>
    /// <param name="move">if we move or not</param>
    private void SetCanMove(bool move) {
        //it can't move and set to move, check the path and start!
        if (move && !canMove) {
            pathFinder.OnTargetChanged();//trigger a recalculation of path

        } else if (!move && canMove) {
            //stop moving, stop the routine
            StopCoroutine("FollowPath");
        }

        canMove = move;
    }

    /// <summary>
    /// If the path changes, stop every movement and start a new one
    /// </summary>
    private void OnChangedPath() {
        #if DEBUG_MovementComponent
        log.Log("Changed path.");
        #endif
        //get new path
        path = pathFinder.GetPath();
        if (path != null && path.Length > 0) {
            StopCoroutine("FollowPath");
            CleanPath();
            StartCoroutine("FollowPath");

        }else if (path == null) {
            #if DEBUG_MovementComponent
            log.Log("There is no more path.");
            #endif
            StopCoroutine("FollowPath");
            CleanPath();
        }
    }

    /// <summary>
    /// Clear moving state
    /// </summary>
    private void CleanPath() {
        targetIndex = 0;
        moving = false;
        Animations(Vector3.zero);
    }

    /// <summary>
    /// We made our way to achieve the target
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPath() {
        //start with the begining
        Vector3 currentWaypoint = path[0];
        //we are moving
        moving = true;

        while (canMove) {
            //if achieved target...
            if (transform.position == currentWaypoint) {
                //get next target
                ++targetIndex;
                //if we arrive at the end of the path...
                if (targetIndex >= path.Length) {
                    //clean everything and end
                    path = new Vector3[0];
                    CleanPath();
                    yield break;
                }
                //get the target position
                currentWaypoint = path[targetIndex];
            }
            //set the animation
            Animations(currentWaypoint);
            //move the gameobject
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, velocity * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Given the actual state of movement, set the apropiate animations
    /// </summary>
    /// <param name="destiny">destination of game object</param>
    private void Animations(Vector3 destiny) {
        if (animator != null) {
            Vector3 actualDirection = (destiny - transform.position).normalized;

            //if moving....
            if (actualDirection.x != moveDirection.x || actualDirection.y != moveDirection.y) {
                moveDirection = actualDirection;
                moveDirection.z = 0;

                animator.SetFloat("DirectionX", moveDirection.x);
                animator.SetFloat("DirectionY", moveDirection.y);
                
                animator.SetBool("Walking", moving);
            }
        }
    }

#if DEBUG_MovementComponent
    public void OnDrawGizmos() {
        //we draw the path from the actual position to the target
        if (path != null) {
            for (int i = targetIndex; i < path.Length; ++i) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector3.one*0.2f);

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
#endif
}
