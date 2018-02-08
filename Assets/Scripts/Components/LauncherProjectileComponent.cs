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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Launch(Vector3 direction) {
        if (prefabProjectile == null) return;

        //create the proyectile
        //give it the direction
        //it will have the velocity and a KillOnCollisionComponent (under folder collisionsComponents)
        GameObject projectile = Instantiate(prefabProjectile);
        projectile.transform.position = transform.position + offsetLauncher;
        projectile.GetComponent<ProjectileComponent>().Direction = direction;
        //get proyectile component and set direction
        GameControllerTemporal.AddTemporal(projectile);
    }
}
