using UnityEngine;

public class LookingComponent : MonoBehaviour {

    private Vector3 moveDirection;
    /// <summary>
    /// Components needed
    /// </summary>
    private Animator animator;
    private TargetComponent target;

    public void Start() {
        animator = GetComponent<Animator>();
        target = GetComponent<TargetComponent>();
        target.RegisterOnTargetChange(OnTargetChanged);
        target.RegisterOnTargetMove(OnTargetChanged);
    }

    public void Look(Vector3 direction) {
        Animations(direction);
    }

    private void OnTargetChanged() {
        Vector3 destiny = target.GetLastTargetPosition();
        Animations(destiny);
    }

    private void Animations(Vector3 destiny) {
        if (animator != null) {
            Vector3 actualDirection = (destiny - transform.position).normalized;

            if (actualDirection.x != moveDirection.x || actualDirection.y != moveDirection.y) {
                moveDirection = actualDirection;
                moveDirection.z = 0;

                animator.SetFloat("DirectionX", moveDirection.x);
                animator.SetFloat("DirectionY", moveDirection.y);
            }
        }
    }

    public void OnDrawGizmos() {
        //if (moveDirection != Vector3.zero) {
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(transform.position, transform.position+moveDirection);
        //}
    }
}
