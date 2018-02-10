using UnityEngine;

public class LauncherProjectileComponent : MonoBehaviour {
    /// <summary>
    /// The proyectile to launch
    /// </summary>
    [SerializeField]
    private GameObject prefabProjectile;
    /// <summary>
    /// Adjust it to have the proyectile start in another place
    /// </summary>
    [SerializeField]
    private Vector3 offsetLauncher = Vector3.zero;

    [SerializeField]
    private bool useCustomVelocity = false;


    [SerializeField]
    private float customVelocityProjectile=0;

    
    public void Launch(Vector3 direction) {
        if (prefabProjectile == null) return;

        //create the proyectile
        //give it the direction
        //it will have the velocity and a KillOnCollisionComponent (under folder collisionsComponents)
        GameObject projectile = Instantiate(prefabProjectile);
        projectile.transform.position = transform.position + offsetLauncher;
        ProjectileComponent projectileCmp= projectile.GetComponent<ProjectileComponent>();
        projectileCmp.Direction = direction;
        if (useCustomVelocity) {
            projectileCmp.SetVelocity(customVelocityProjectile);
        }
        //get proyectile component and set direction
        GameControllerTemporal.AddTemporal(projectile);
    }
}
