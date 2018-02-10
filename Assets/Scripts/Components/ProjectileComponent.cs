using UnityEngine;

public class ProjectileComponent : MonoBehaviour {
    /// <summary>
    /// Quantity of damage it could do
    /// </summary>
    [SerializeField]
    private int damage = 0;
    /// <summary>
    /// Velocity of the projectile
    /// </summary>
    [SerializeField]
    private float velocity;
    /// <summary>
    /// Direction of projectile
    /// </summary>
    private Vector3 direction;

    private Transform parent;

    /// <summary>
    /// has collide?
    /// </summary>
    private bool onCollision = false;

    /// <summary>
    /// animator of projectile
    /// </summary>
    private Animator animations;

    public Vector3 Direction {
        get { return direction; }
        set {
            direction = value;
            Vector3 destiny = transform.position + direction;
            float AngleRad = Mathf.Atan2(destiny.y - transform.position.y, destiny.x - transform.position.x);
            float angle = (180 / Mathf.PI) * AngleRad;
            transform.rotation = Quaternion.Euler(0, 0, angle);

        }
    }

    public void SetVelocity(float velocity) {
        this.velocity = velocity;
    }

    public void Start() {
        animations = GetComponent<Animator>();
        parent = transform.parent;
    }

    public void Update() {
        if (!onCollision) {
            parent.transform.Translate(direction * velocity * Time.deltaTime);
        }
    }

    /// <summary>
    /// Specific behaviour on collision with the tag
    /// can be override by extended classes
    /// </summary>
    /// <param name="other"></param>
    virtual public void OnTriggerEnter(Collider other) {
        if (other.tag == "Hero") { //TODO improve tag to use
            if (animations != null) {
                animations.SetTrigger("Explosion");
            } else {
                //destroy the object
                Destroy(gameObject);
            }
            onCollision = true;
            //get object and do damage TODO this is done by another component =>  KillOnCollisionComponent (under folder collisionsComponents)
            LifeComponent life = other.gameObject.GetComponent<LifeComponent>();
            if (life != null) {
                life.Damage(damage);
            }
        }
    }


    /// <summary>
    /// Event from animator, it wait two seconds after the event (at the end of the animation more or less
    /// </summary>
    public void OnExplosion() {
        Destroy(transform.parent.gameObject, 2);
    }
}
