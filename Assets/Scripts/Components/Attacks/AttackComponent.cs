using UnityEngine;

public abstract class AttackComponent : MonoBehaviour {
    /// <summary>
    /// The layer mask of colliders for where the item can't be put down
    /// </summary>
    [SerializeField]
    protected LayerMask[] layerColliders;

    private Animator animator;

    private bool walkingExists = false;

    /// <summary>
    /// The number combining all layers to check in collision
    /// </summary>
    protected int layerCollision;

    public virtual void Start() {
        layerCollision = Utils.ToLayer(layerColliders);
        animator = GetComponent<Animator>();
        if (animator != null) {
            walkingExists = Utils.AnimatorContainsParam(animator, "Walking");
        }
    }

    /// <summary>
    /// make the attack animation trigger
    /// </summary>
    public void AttackAnimation() {
        if (animator != null) {
            if(walkingExists) animator.SetBool("Walking", false);
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    public abstract void Attack(Vector3 direction);
}
