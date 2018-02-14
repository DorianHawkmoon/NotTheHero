//#define DEBUG_Bomb
#define DEBUG_Bomb_GIZMOS


using UnityEngine;

/// <summary>
/// Specific class for a bomb
/// TODO damage function can be virtual so inheritance can be specific mine field for other effects instead of just damage
/// </summary>
public class Bomb : MonoBehaviour {

    /// <summary>
    /// Timelife of bomb
    /// </summary>
    [SerializeField]
    private float time;

    /// <summary>
    /// Radius of explosion
    /// </summary>
    [SerializeField]
    private float radiusExplosion = 1;

    /// <summary>
    /// damage it does
    /// </summary>
    [SerializeField]
    private int damage = 1;

    /// <summary>
    /// prefab of explosion
    /// </summary>
    [SerializeField]
    private GameObject explosionPrefab = null;

    /// <summary>
    /// scale for the explosion
    /// </summary>
    [SerializeField]
    private float scaleExplosion = 1;

    /// <summary>
    /// Timer for the timelife of bomb
    /// </summary>
    private float timer;

    /// <summary>
    /// Set the timer and get the animator if exists
    /// </summary>
    public void Start() {
        timer = time;

        Animator anim = GetComponent<Animator>();
        if (anim != null) {
            //Get the length of the animation and adjust it for the timelife of bomb
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            if (clips.Length > 0) {
                float speed = 1.0f / (time / clips[0].length);
                anim.SetFloat("Speed", speed);
            }
        }
    }

    /// <summary>
    /// Check lifetime of bomb
    /// </summary>
    public void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            #if DEBUG_Bomb
            Debug.Log("Bomb timer end and explode.");
            #endif
            Explode();
        }
    }

    /// <summary>
    /// Set the explosion and destroy the gameobject
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

        foreach (Collider collider in list) {
            GameObject gameObject = collider.gameObject;
            if (gameObject.tag == "Hero") { //TODO improve use of tags
                gameObject.GetComponent<LifeComponent>().Damage(damage);
            }
        }

    }

#if DEBUG_Bomb_GIZMOS
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }
#endif

}
