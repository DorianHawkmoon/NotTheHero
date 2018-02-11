//#define DEBUG_LookingComponent

using UnityEngine;

public class LookingComponent : MonoBehaviour {
#if DEBUG_LookingComponent
    private static DebugLog log = new DebugLog("LookingComponent");
#endif

    /// <summary>
    /// The direction is looking for
    /// </summary>
    private Vector3 lookDirection;
    /// <summary>
    /// Components needed for change animations and the target it should look at
    /// </summary>
    private Animator animator;
    private TargetComponent target;

    /// <summary>
    /// Get the components and register to target changes
    /// </summary>
    public void Start() {
        animator = GetComponent<Animator>();
        target = GetComponent<TargetComponent>();
        if (target != null) {
            target.RegisterOnTargetChange(OnTargetChanged);
            target.RegisterOnTargetMove(OnTargetChanged);
        }

        #if DEBUG_LookingComponent
        else {
            log.Log("No target component.");
        }
        if (animator == null) {
            log.Log("No animator component.");
        }
        log.Log("Looking component initialized.");
        #endif
    }

    /// <summary>
    /// Change the look direction
    /// </summary>
    /// <param name="direction">direction it should look</param>
    public void Look(Vector3 direction) {
        #if DEBUG_LookingComponent
        log.Log("Look.");
        #endif
        Animations(direction);
    }

    /// <summary>
    /// When the target has changed or moved update the look direction
    /// </summary>
    private void OnTargetChanged() {
        #if DEBUG_LookingComponent
        log.Log("Target changed.");
        #endif
        Vector3 destiny = target.GetLastTargetPosition();
        Animations(destiny);
    }

    /// <summary>
    /// Take the new look direction and update animations
    /// </summary>
    /// <param name="destiny"></param>
    private void Animations(Vector3 destiny) {
        if (animator != null) {
            Vector3 actualDirection = (destiny - transform.position).normalized;

            if (actualDirection.x != lookDirection.x || actualDirection.y != lookDirection.y) {
                lookDirection = actualDirection;
                lookDirection.z = 0;

                animator.SetFloat("DirectionX", lookDirection.x);
                animator.SetFloat("DirectionY", lookDirection.y);
            }
        }
    }

#if DEBUG_LookingComponent
    public void OnDrawGizmos() {
        //Draw the direction is looking for
        if (lookDirection != Vector3.zero) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position+lookDirection);
        }
    }
#endif
}
