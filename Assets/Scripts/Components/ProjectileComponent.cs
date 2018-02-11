﻿//#define DEBUG_ProjectileComponent

using UnityEngine;

//TODO improve: made this a vase class of projectile and inherit for diferent kind of projetiles
//one for doing just damage, others for doing confusion and so on...

/// <summary>
/// Script for any kind of projectile who do damage on collision
/// </summary>
public class ProjectileComponent : MonoBehaviour {
#if DEBUG_ProjectileComponent
    private static DebugLog log = new DebugLog("ProjectileComponent");
#endif

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

    /// <summary>
    /// A projectile is made of one gameobject, who moves, 
    /// and a child gameobject who rotates and have this script and collider
    /// The parent is the main gameobject
    /// </summary>
    private Transform parent;

    /// <summary>
    /// has collide?
    /// </summary>
    private bool onCollision = false;

    /// <summary>
    /// animator of projectile
    /// </summary>
    private Animator animations;
    
    /// <summary>
    /// Set the direction of the projectile so it rotate as needed to face the direction
    /// </summary>
    /// <param name="value">Direction to face</param>
    public void SetDirection(Vector3 value) {
        #if DEBUG_ProjectileComponent
        log.Log("Setted direction: "+value+".");
        #endif
        direction = value;
        Vector3 destiny = transform.position + direction;
        float AngleRad = Mathf.Atan2(destiny.y - transform.position.y, destiny.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Set the velocity of the projectile
    /// </summary>
    public float Velocity {
        set { velocity = value; }
    }

    /// <summary>
    /// Get needed elements
    /// </summary>
    public void Start() {
        animations = GetComponent<Animator>();
        parent = transform.parent;
    }

    /// <summary>
    /// Move the projectile
    /// </summary>
    public void Update() {
        if (!onCollision) {
            parent.transform.Translate(direction * velocity * Time.deltaTime);
        }
    }

    /// <summary>
    /// Specific behaviour on collision with the tag
    /// can be override by extended classes
    /// </summary>
    /// <param name="other">collider</param>
    virtual public void OnTriggerEnter(Collider other) {
        if (other.tag == "Hero") { //TODO improve tag to use
            #if DEBUG_ProjectileComponent
            log.Log("Collision.");
            #endif

            if (animations != null) {
                #if DEBUG_ProjectileComponent
                log.Log("Explosion animation.");
                #endif
                animations.SetTrigger("Explosion");
            } else {
                #if DEBUG_ProjectileComponent
                log.Log("No animation, destroying.");
                #endif
                //destroy the object
                Destroy(gameObject);
            }
            onCollision = true;
            //get object and do damage TODO this is done by another component =>  KillOnCollisionComponent (under folder collisionsComponents)
            LifeComponent life = other.gameObject.GetComponent<LifeComponent>();
            if (life != null) {
                #if DEBUG_ProjectileComponent
                log.Log("Doing damage.");
                #endif
                life.Damage(damage);
            }
        }
    }

    /// <summary>
    /// Event from animator, it wait two seconds after the event (at the end of the animation more or less)
    /// </summary>
    public void OnExplosion() {
        #if DEBUG_ProjectileComponent
        log.Log("Destroying in 2 seconds.");
        #endif
        Destroy(transform.parent.gameObject, 2);
    }
}
