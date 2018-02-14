//#define DEBUG_SearchKillDistance

using UnityEngine;

/// <summary>
/// Class for a game object who will look and go to a target, and at some distance
/// it will attack it until its dead or go out of range
/// Will look for the nearest target
/// </summary>
public class SearchKillDistance : MonoBehaviour {

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
    /// Type of targets to look for //TODO improve tag treatment
    /// </summary>
    [TagSelector]
    [SerializeField]
    private string tagTarget = "";
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

    /// <summary>
    /// Needed component to set the target and move
    /// </summary>
    private TargetComponent targetComponent;
    private MovementComponent moveComponent;
    private LauncherProjectileComponent launcherComponent;

    /// <summary>
    /// Get needed components
    /// </summary>
    public void Start() {
        targetSelected = null;
        targetComponent = GetComponent<TargetComponent>();
        moveComponent = GetComponent<MovementComponent>();
        launcherComponent = GetComponent<LauncherProjectileComponent>();
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
        if (!CadenceShoot() && !inCadence) {  //if haven't shoot and not waiting(cadence)...
            //check for the target, the distance and shoot if posible
            CheckTarget();
            CheckDistanceShoot();
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
                #if DEBUG_SearchKillDistance
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
    /// Check the nearest target
    /// </summary>
    private void CheckTarget() {
        //TODO measure performance (quadtree of heroes?) or quadtree of lifecomponent and filter by tag
        GameObject[] targetsFounds = GameObject.FindGameObjectsWithTag(tagTarget);
        GameObject target = GetClosestEnemy(targetsFounds);

        if (target == null){
            if(targetComponent.HasTarget()) targetComponent.SetTargetObject(null);
            targetSelected = null;
            return;
        }

        //if found a target, set it and watch its life (if someone else kill it)
        if (targetSelected == null || targetSelected != target) {
            #if DEBUG_SearchKillDistance
            Debug.Log("Target selected.");
            #endif

            targetSelected = target;
            targetComponent.SetTargetObject(targetSelected);
            targetSelected.GetComponent<LifeComponent>().RegisterOnDeath(OnTargetDeath);
        }
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
            #if DEBUG_SearchKillDistance
            Debug.Log("Target close, shooting it.");
            #endif

            moveComponent.StopMovement();
            launcherComponent.Launch(directionToTarget);
            result = true;
            //start cadence count
            inCadence = true;
            timerCadence = cadence;
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
        for(int i=0; i<enemies.Length; ++i){
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

    /// <summary>
    /// If the target die, clean it
    /// </summary>
    private void OnTargetDeath() {
        if (targetSelected != null) {
            #if DEBUG_SearchKillDistance
            Debug.Log("Target is dead.");
            #endif

            targetSelected.GetComponent<LifeComponent>().UnregisterOnDeath(OnTargetDeath);
            targetSelected = null;
            targetComponent.SetTargetObject(null);
        }
    }
}
