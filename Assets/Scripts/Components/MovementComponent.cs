using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour {
    public float velocity = 1;

    private bool canMove = true;
    private Vector3[] path;
    private int targetIndex;
    private Vector3 moveDirection;

    /// <summary>
    /// Components needed
    /// </summary>
    private PathFinderComponent pathFinder;
    private Animator animator;


    public void Start() {
        animator = GetComponent<Animator>();
        pathFinder = GetComponent<PathFinderComponent>();
        pathFinder.RegisterOnPathChange(OnChangedPath);
    }

    public void Update() {

    }

    public void StopMovement() {
        SetCanMove(false);
    }

    public void MoveToTarget() {
        SetCanMove(true);
    }

    public void CanMove() {
        canMove = true;
    }

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

    private void OnChangedPath() {
        //get new path
        path = pathFinder.GetPath();
        if (path != null && path.Length > 0) {
            StopCoroutine("FollowPath");
            CleanPath();
            StartCoroutine("FollowPath");
        }
    }

    private void CleanPath() {
        targetIndex = 0;
        moveDirection = Vector3.zero;
        Animations(Vector3.zero);
    }

    private IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (canMove) {
            if (transform.position == currentWaypoint) {
                ++targetIndex;
                if (targetIndex >= path.Length) {
                    path = new Vector3[0];
                    CleanPath();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            
            Animations(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, velocity * Time.deltaTime);
            yield return null;
        }
    }

    private void Animations(Vector3 destiny) {
        if (animator != null) {
            Vector3 actualDirection = (destiny - transform.position).normalized;

            if (actualDirection.x != moveDirection.x || actualDirection.y != moveDirection.y) {
                moveDirection = actualDirection;
                moveDirection.z = 0;

                animator.SetFloat("DirectionX", moveDirection.x);
                animator.SetFloat("DirectionY", moveDirection.y);

                animator.SetBool("FaceUp", moveDirection.y > 0);
                animator.SetBool("FaceDown", moveDirection.y < 0);

                animator.SetBool("FaceRight", moveDirection.x > 0);
                animator.SetBool("FaceLeft", moveDirection.x < 0);

                animator.SetBool("Walking", moveDirection != Vector3.zero);
            }
        }
    }

    public void OnDrawGizmos() {
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
}
