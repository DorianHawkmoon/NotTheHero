//#define DEBUG_MineField
#define DEBUG_MineField_GIZMOS

using UnityEngine;

/// <summary>
/// TODO los tags pueden ser una simple clase que guarda la lista de tags, eso permite un inspector
/// personalizado (teoricamente) y a la hora de comprobar los tags, ya tendría una función que me lo chequea
/// 
/// TODO damage function can be virtual so inheritance can be specific mine field for other effects instead of just damage
/// </summary>

/// <summary>
/// Specific class for a mine field
/// </summary>
public class MineField : MonoBehaviour {
#if DEBUG_MineField
    private static DebugLog log = new DebugLog("MineField");
#endif

    /// <summary>
    /// Little delay for the mine to activate
    /// </summary>
    [SerializeField]
    private float timeToActivate;

    /// <summary>
    /// Radius of explosion
    /// </summary>
    [SerializeField]
    private float radiusExplosion = 1;

    /// <summary>
    /// Damage it does
    /// </summary>
    [SerializeField]
    private int damage = 1;

    /// <summary>
    /// The prefab of explosion
    /// </summary>
    [SerializeField]
    private GameObject explosionPrefab = null;

    /// <summary>
    /// The scale for the explosion
    /// </summary>
    [SerializeField]
    private float scaleExplosion = 1;

    /// <summary>
    /// Timer for the activation delay
    /// </summary>
    private float timer;

    /// <summary>
    /// If the mine is activated
    /// </summary>
    private bool activated=false;

    /// <summary>
    /// Set the timer of activation
    /// </summary>
    public void Start() {
        timer = timeToActivate;
    }

    /// <summary>
    /// Check the timer of activation
    /// </summary>
    public void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            activated = true;
        }
    }

    /// <summary>
    /// If someone gets inside the trigger of the mine, it explode
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (activated) {
            if (other.gameObject.tag == "Hero") { //TODO tags
                #if DEBUG_MineField
                log.Log("Mine triggered.");
                #endif
                Explode();
            }
        }
    }

    /// <summary>
    /// Create the explosion, do the damage and destroy this object
    /// </summary>
    private void Explode() {
        //do damage
        Damage();
        //do the explosion
        if (explosionPrefab != null) {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            explosion.GetComponent<Explosion>().SetScaleExplosion(scaleExplosion);
            GameControllerTemporal.AddTemporal(explosion);
        }

        //destroy itself
        Destroy(gameObject);
    }

    /// <summary>
    /// Do the damage
    /// </summary>
    private void Damage() {
        Collider[] list = Physics.OverlapSphere(transform.position, radiusExplosion);
        for (int i = 0; i < list.Length; ++i) {
            GameObject gameObject = list[i].gameObject;
            if (gameObject.tag == "Hero") { //TODO improve use of tags
                gameObject.GetComponent<LifeComponent>().Damage(damage);
            }
        }
    }

#if DEBUG_MineField_GIZMOS
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }
#endif
}
