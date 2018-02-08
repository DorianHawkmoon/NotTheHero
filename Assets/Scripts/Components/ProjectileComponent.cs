using UnityEngine;

public class ProjectileComponent : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private int damage;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float velocity;
    /// <summary>
    /// 
    /// </summary>
    private Vector3 direction;

    private bool onCollision=false;

    private Animator animations;

    public Vector3 Direction {
        get { return direction; }
        set { direction = value; }
    }

    public void Start() {
        animations = GetComponent<Animator>();
    }

    public void Update() {
        if (!onCollision) {
            transform.Translate(direction * velocity * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Hero") { //TOOD improve tag to use
            Debug.Log("Explosion");
            onCollision = true;
            animations.SetTrigger("Explosion");
            //get object and do damage
        }
    }

    public void OnExplosion() {
        Destroy(this.gameObject, 2);
    }
}
