#define DEBUG_KillDistance

using UnityEngine;

/// <summary>
/// Class for a game object which will look for any target inside an area and shoot it
/// </summary>
public class KillDistance : MonoBehaviour {

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
    /// Type of targets to look for
    /// </summary>
    [TagSelector]
    [SerializeField]
    private string tagTarget = "";

    /// <summary>
    /// Minimum value to wait until change its look direction
    /// </summary>
    [SerializeField]
    private float minTimeRandomLook;
    /// <summary>
    /// Maximum value to wait until change its look direction
    /// </summary>
    [SerializeField]
    private float maxTimeRandomLook;

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
    /// Timer for random looking
    /// </summary>
    private float timerRandomLooking;

    /// <summary>
    /// Needed component to set the target and shoot
    /// </summary>
    private TargetComponent targetComponent;
    private LauncherProjectileComponent launcherComponent;
    private LookingComponent lookingComponent;

    /// <summary>
    /// Get components needed
    /// </summary>
    public void Start() {
        //seed random
        Random.InitState(System.Environment.TickCount);
        targetSelected = null;
        timerRandomLooking = Random.Range(minTimeRandomLook, maxTimeRandomLook);
        targetComponent = GetComponent<TargetComponent>();
        launcherComponent = GetComponent<LauncherProjectileComponent>();
        lookingComponent = GetComponent<LookingComponent>();

        targetComponent.RegisterOnTargetMove(OnTargetMoved);
    }

    /// <summary>
    /// Various situations
    /// At the begining, the character can move, will look for a target and go to him,
    /// if it reach the target, it stop moving and shoot
    /// 
    /// After the shoot, it wait for a cadence. When the cadence is over
    /// it check for the same target and shoot again if close
    /// else it set the movement and look for another target
    /// 
    /// </summary>
    public void Update() {
        if (!CadenceShoot() && !inCadence) {  //if haven't shoot and not waiting(cadence)...
            //check for the target, the distance and shoot if posible
            CheckTarget();
            CheckDistanceShoot();
        }

        if (targetSelected == null) {
            timerRandomLooking -= Time.deltaTime;
            if (timerRandomLooking < 0 && lookingComponent!=null) {
                timerRandomLooking = Random.Range(minTimeRandomLook, maxTimeRandomLook);
                lookingComponent.Look(new Vector3(Random.value, Random.value));
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
                #if DEBUG_KillDistance
                Debug.Log("Timer cadence over.");
                #endif
                //check the same target again to see if still close
                if (CheckDistanceShoot()) {
                    //it has shoot, start cadence again
                    timerCadence = cadence;
                    result = true;
                } else {
                    inCadence = false;
                    targetSelected = null;
                    targetComponent.SetTargetObject(null);
                    timerRandomLooking = Random.Range(minTimeRandomLook, maxTimeRandomLook);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Check if there is any target near
    /// </summary>
    private void CheckTarget() {
        Collider[] targetsRadius = Physics.OverlapSphere(transform.position, distanceToShoot);
        GameObject target = GetClosestEnemy(targetsRadius);

        if (target == null) return;

        if (targetSelected == null || targetSelected != target) {
            #if DEBUG_KillDistance
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
        #if DEBUG_KillDistance
            Debug.Log("Target close, shooting it.");
        #endif
        launcherComponent.Launch(directionToTarget);
        result = true;
        //start cadence count
        inCadence = true;
        timerCadence = cadence;

        return result;
    }

    /// <summary>
    /// Get the nearest enemy given a list of potential targets
    /// </summary>
    /// <param name="enemies">array with potential targets</param>
    /// <returns>The closest enemy or null if array is null</returns>
    private GameObject GetClosestEnemy(Collider[] enemies) {
        GameObject target = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider potentialCollider in enemies) {
            GameObject potential = potentialCollider.gameObject;
            if (potential.tag != tagTarget) continue;

            LifeComponent life = potential.GetComponent<LifeComponent>(); //TODO urgent improve!!
            if (life == null || life.IsDead()) continue;

            Transform potentialTarget = potential.transform;
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
    /// When the target moves, follow it with the eyes (if still inside the range)
    /// </summary>
    private void OnTargetMoved() {
        if (targetSelected == null) return;

        //check the distance of the target, if not close, the target has move out of range
        Transform potentialTarget = targetSelected.transform;
        Vector3 directionToTarget = potentialTarget.position - transform.position;
        float dSqrToTarget = directionToTarget.sqrMagnitude;
        //if not near, forget it
        if (dSqrToTarget + 1 > distanceToShoot * distanceToShoot) {
            targetSelected = null;
            targetComponent.SetTargetObject(null);
            timerRandomLooking = Random.Range(minTimeRandomLook, maxTimeRandomLook);
        }
    }

    /// <summary>
    /// If the target die, clean it
    /// </summary>
    private void OnTargetDeath() {
        if (targetSelected != null) {
            targetSelected.GetComponent<LifeComponent>().UnregisterOnDeath(OnTargetDeath);
        }
        targetSelected = null;
        timerRandomLooking = Random.Range(minTimeRandomLook, maxTimeRandomLook);
    }

}
