using UnityEngine;

public class ProjectileComponent : MonoBehaviour {
    /// <summary>
    /// Quantity of damage it could do
    /// </summary>
    [SerializeField]
    private int damage=0;
    /// <summary>
    /// Velocity of the projectile
    /// </summary>
    [SerializeField]
    private float velocity;
    /// <summary>
    /// Direction of projectile
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// has collide?
    /// </summary>
    private bool onCollision=false;

    /// <summary>
    /// animator of projectile
    /// </summary>
    private Animator animations;

    public Vector3 Direction {
        get { return direction; }
        set { direction = value; }
    }

    public void SetVelocity(float velocity) {
        this.velocity = velocity;
    }

    public void Start() {
        animations = GetComponent<Animator>();
    }

    public void Update() {
        if (!onCollision) {
            transform.Translate(direction * velocity * Time.deltaTime);
        }
    }

    /// <summary>
    /// Specific behaviour on collision with the tag
    /// can be override by extended classes
    /// </summary>
    /// <param name="other"></param>
    virtual public void OnTriggerEnter(Collider other) {
        if (other.tag == "Hero") { //TODO improve tag to use
            animations.SetTrigger("Explosion");
            onCollision = true;
            //get object and do damage
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
        Destroy(this.gameObject, 2);
    }
}
