//#define DEBUG_HeroBehaviour

using UnityEngine;

/// <summary>
/// This class control the target of the hero
/// First the tower, if not, the player tower
/// Sometimes the target changes because of confusion (traps, bombs, enemies...)
/// </summary>
public class HeroBehaviour : MonoBehaviour {
    /// <summary>
    /// At what distance it start shooting
    /// </summary>
    [SerializeField]
    private float distanceToShoot = 1;
    /// <summary>
    /// Cadence of shooting
    /// </summary>
    [SerializeField]
    private float cadence = 1;
    
    /// <summary>
    /// The actual target (life component)
    /// </summary>
    private LifeComponent lifeTargeted;

    /// <summary>
    /// The timer for the cadence
    /// </summary>
    private float timerCadence = 0;
    /// <summary>
    /// If in cadence
    /// </summary>
    private bool inCadence = false;
    /// <summary>
    /// Target is going through
    /// </summary>
    private GameObject targetSelected; //TODO improve this by getting a targetable component which specify the exact point
                                       //to shoot and look for

    //the hero has died
    private bool died;

    /// <summary>
    /// Needed components
    /// </summary>
    private TargetComponent targetComponent;
    private MovementComponent moveComponent;
    private AttackComponent launcherComponent;
    private LifeComponent lifeComponent;

    // Use this for initialization
    public void Start() {
        targetComponent = GetComponent<TargetComponent>();
        moveComponent = GetComponent<MovementComponent>();
        launcherComponent = GetComponent<AttackComponent>();
        lifeComponent = GetComponent<LifeComponent>();

        Debug.Assert(targetComponent != null);
        Debug.Assert(moveComponent != null);

        if (lifeComponent != null) {
            lifeComponent.RegisterOnDeath(OnHeroDeath);
        }

        if (!SearchTower()) {
            SearchPlayerTower();
        }
    }

    /// <summary>
    /// Various situations
    /// At the begining, the character can move, will look for a target and go to him,
    /// if it reach the target, it stop moving and shoot
    /// 
    /// After the shoot, it wait for a cadence. When the cadence is over
    /// it check for the same target and shoot again if close
    /// else it set the movement and look for another target
    /// </summary>
    public void Update() {
        if (died) return;

        if (!CadenceShoot() && !inCadence) {
            CheckTarget();
            CheckDistanceShoot();
        }
    }

    /// <summary>
    /// Check the nearest target
    /// </summary>
    private void CheckTarget() {
        if (!SearchTower()) { //TODO check performance
            SearchPlayerTower();
        }

        if (targetSelected == null) {
            if (targetComponent.HasTarget()) targetComponent.SetTargetObject(null);
            if (lifeTargeted != null) {
                lifeTargeted.UnregisterOnDeath(OnTargetDeath);
                lifeTargeted = null;
            }
        }
    }

    /// <summary>
    /// Check if can shoot again and shoot if the target is still close
    /// </summary>
    /// <returns>True if had shoot, false otherwise</returns>
    private bool CadenceShoot() {
        bool result = false;
        if (inCadence) {
            timerCadence -= Time.deltaTime;

            if (timerCadence < 0) {
                #if DEBUG_HeroBehaviour
                Debug.Log("Timer cadence over.");
                #endif
                //check the same target again to see if still close
                if (CheckDistanceShoot()) {
                    //it has shoot, start cadence again
                    timerCadence = cadence;
                    result = true;
                } else {
                    inCadence = false;
                    moveComponent.CanMove();
                    targetSelected = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Check the distance with the target and if
    /// </summary>
    /// <returns>True if it was close and have shoot</returns>
    private bool CheckDistanceShoot() {
        bool result = false;

        if (targetSelected == null) return result;

        Transform potentialTarget = targetSelected.transform;
        Vector3 directionToTarget = potentialTarget.position - transform.position;
        float dSqrToTarget = directionToTarget.sqrMagnitude;
        //if near, stop and shoot it
        if (dSqrToTarget <= distanceToShoot * distanceToShoot) {
#if DEBUG_HeroBehaviour
            Debug.Log("Target close, shooting it.");
#endif

            moveComponent.StopMovement();
            launcherComponent.Attack(directionToTarget);
            result = true;
            //start cadence count
            inCadence = true;
            timerCadence = cadence;
        }

        return result;
    }

    /// <summary>
    /// Here we set another target as a confusion
    /// after it dies or after some time, we recover the initial target (tower or player tower)
    /// </summary>
    public void ChangeTargetConfusion() {
        //TODO set another target until die or timer
    }

    /// <summary>
    /// Search for a tower point of attack and set it as a target
    /// </summary>
    /// <returns>true if we setted a tower point of attack</returns>
    private bool SearchTower() {
        bool result = false;

        //check if we already have a tower in target
        bool checkNewTower = false;
        checkNewTower |= targetSelected != targetComponent.GetTarget();
        checkNewTower |= targetSelected == null;
        checkNewTower |= lifeTargeted != null && lifeTargeted.IsDead();
        checkNewTower |= lifeTargeted == null;
        if (!checkNewTower) return true;

        //get the nearest point attack of tower
        GameObject[] towerPointAttacks = GameObject.FindGameObjectsWithTag("Tower");
        GameObject pointAttack = GetClosestEnemy(towerPointAttacks);
        LifeComponent life = (pointAttack != null) ? pointAttack.GetComponent<LifeComponent>() : null;

        result = life != null && !life.IsDead();

        if (result) {
            #if DEBUG_HeroBehaviour
            Debug.Log("Target tower selected.");
            #endif
            if (targetSelected != pointAttack) {
                targetSelected = pointAttack;
                targetComponent.SetTargetObject(targetSelected);
                lifeTargeted = life;
                lifeTargeted.RegisterOnDeath(OnTargetDeath);
            }
        } else {
            targetSelected = null;
        }
        return result;
    }

    /// <summary>
    /// Search for the tower player
    /// </summary>
    /// <returns>true if we setted it as target</returns>
    private bool SearchPlayerTower() {
        bool result = false;

        bool checkNewTarget = false;
        checkNewTarget |= targetSelected == null || lifeTargeted == null;
        checkNewTarget |= lifeTargeted != null && lifeTargeted.IsDead();
        if (!checkNewTarget) return true;

        GameObject pointAttack = GameObject.FindGameObjectWithTag("Tower Player");
        LifeComponent life = (pointAttack != null) ? pointAttack.GetComponent<LifeComponent>() : null;

        result = life != null && !life.IsDead();

        if (result) {
            #if DEBUG_HeroBehaviour
            Debug.Log("Target tower player selected.");
            #endif
            if (targetSelected != pointAttack) {
                targetSelected = pointAttack;
                targetComponent.SetTargetObject(targetSelected);
                lifeTargeted = life;
                lifeTargeted.RegisterOnDeath(OnTargetDeath);
            }
        } else {
            targetSelected = null;
        }
        return result;
    }


    /// <summary>
    /// Check what target is near given a list of potential targets
    /// </summary>
    /// <param name="enemies">array with potential targets</param>
    /// <returns>The closest enemy or null if array is null</returns>
    private GameObject GetClosestEnemy(GameObject[] enemies) {
        GameObject target = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < enemies.Length; ++i) {
            GameObject potential = enemies[i];
            if (potential == this.gameObject) continue;

            Transform potentialTarget = potential.transform;
            LifeComponent life = potential.GetComponent<LifeComponent>(); //TODO urgent improve!!
            if (life == null || life.IsDead()) continue;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                target = potential;
            }
        }

        return target;
    }

    private void OnHeroDeath() {
        lifeComponent.UnregisterOnDeath(OnHeroDeath);
        died = true;

        //TODO do death animation (in other component I guess)
        //destroy enemy
        Destroy(this.gameObject, 5);
    }

    /// <summary>
    /// If the target die, look for the next target
    /// </summary>
    private void OnTargetDeath() {
#if DEBUG_HeroBehaviour
        Debug.Log("Target death.");
#endif
        targetSelected = null;
        lifeTargeted = null;
        if (!SearchTower()) {
            SearchPlayerTower();
        }
    }
    

}
