using UnityEngine;

/// <summary>
/// TODO los tags pueden ser una simple clase que guarda la lista de tags, eso permite un inspector
/// personalizado (teoricamente) y a la hora de comprobar los tags, ya tendría una función que me lo chequea
/// </summary>

public class MineField : MonoBehaviour {


    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float timeToActivate;
    [SerializeField]
    private float radiusExplosion = 1;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private int damage = 1;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject explosionPrefab = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float scaleExplosion = 1;

    private float timer;
    private bool activated=false;
    public void Start() {
        timer = timeToActivate;
    }

    public void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            activated = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (activated) {
            if (other.gameObject.tag == "Hero") { //TODO tags
                Explode();
            }
        }
    }

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

    private void Damage() {
        Collider[] list = Physics.OverlapSphere(transform.position, radiusExplosion);

        foreach (Collider collider in list) {
            GameObject gameObject = collider.gameObject;
            if (gameObject.tag == "Hero") { //TODO improve use of tags
                gameObject.GetComponent<LifeComponent>().Damage(damage);
            }
        }

    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }
}
