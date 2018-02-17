#define DEBUG_CloseAttackComponent

using System.Collections.Generic;
using UnityEngine;

public class CloseAttackComponent : AttackComponent {

    /// <summary>
    /// The proyectile to launch
    /// </summary>
    [SerializeField]
    private int damage;

    /// <summary>
    /// Radius of action
    /// </summary>
    [SerializeField]
    private float radius;

    /// <summary>
    /// Grades of the angle in which make damage
    /// </summary>
    [SerializeField]
    private float angleGradeAttack;

    /// <summary>
    /// Distance between the raycast to find objects
    /// </summary>
    private float segmentsDistance = 0.2f;

    /// <summary>
    /// 
    /// </summary>
    private HashSet<LifeComponent> attacked;

    public override void Start() {
        base.Start();
        attacked = new HashSet<LifeComponent>();
    }

    /// <summary>
    /// Create and launch the projectile
    /// </summary>
    /// <param name="direction"></param>
    public override void Attack(Vector3 direction) {
        #if DEBUG_CloseAttackComponent
        Debug.Log("Corporal attack");
        #endif

        attacked.Clear();
        Vector3 startPos = transform.position;
        Vector3 targetPos = Vector3.zero;

        int startAngle = (int)(-angleGradeAttack * 0.5); // half the angle to the Left
        int finishAngle = (int)(angleGradeAttack * 0.5); // half the angle to the Right
        #if DEBUG_CloseAttackComponent
        Debug.Log(startAngle);
        Debug.Log(finishAngle);
        #endif

        float b = radius;
        float c = radius;
        float distanceCO = Mathf.Sqrt(b * b + c * c - 2 * b * c * Mathf.Cos(angleGradeAttack));
        int segments = Mathf.RoundToInt(Mathf.Ceil(distanceCO / segmentsDistance));
        
        AttackAnimation();

        // the gap between each ray (increment)
        int inc = (int)(angleGradeAttack / segments);
        RaycastHit hit;

        // step through and find each target point
        for (int i = startAngle; i <= finishAngle; i += inc) // Angle from forward
        {
            targetPos = startPos + (Quaternion.Euler(0, 0, i) * direction).normalized * radius;

            // linecast between points
            if (Physics.Linecast(startPos, targetPos, out hit, layerCollision)) {
                #if DEBUG_CloseAttackComponent
                Debug.Log("Hit " + hit.collider.gameObject.name);
                #endif
                //get object and do damage
                LifeComponent life = hit.collider.gameObject.GetComponent<LifeComponent>();
                if (life != null) {
                    if (attacked.Add(life)) {
                        #if DEBUG_CloseAttackComponent
                        Debug.Log("Doing damage.");
                        #endif
                        life.Damage(damage);
                    }
                }
            }

            #if DEBUG_CloseAttackComponent
            // to show ray just for testing
            Debug.DrawLine(startPos, targetPos, Color.green, 5);
            #endif 
        }

        
    }
}
