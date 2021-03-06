﻿//#define DEBUG_LauncherProjectileComponent

using UnityEngine;

/// <summary>
/// A launcher which creates the projectile and launch them
/// </summary>
public class LauncherProjectileComponent : AttackComponent {
    /// <summary>
    /// Use the animation event to sync the launch of the projectile
    /// </summary>
    public bool launchOnEventAnimation=true;
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

    /// <summary>
    /// If I want to use a custom velocity instead of the default velocity
    /// of the projectile configured
    /// </summary>
    [SerializeField]
    private bool useCustomVelocity = false;

    /// <summary>
    /// The custom velocity to use for the projectile
    /// if the useCustomVelocity is checked
    /// </summary>
    [SerializeField]
    private float customVelocityProjectile=0;

    /// <summary>
    /// Direction for the projectile
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// Create and launch the projectile
    /// </summary>
    /// <param name="direction"></param>
    public override void Attack(Vector3 direction) {
        if (prefabProjectile == null) return;
        this.direction = direction;

        #if DEBUG_LauncherProjectileComponent
        Debug.Log("Triggering animation.");
        #endif
        AttackAnimation();
        if (!launchOnEventAnimation) {
            Launch();
        }
    }

    public void LaunchProjectileAnimation() {
#if DEBUG_LauncherProjectileComponent
        Debug.Log("Animation event triggered.");
#endif        
        if (launchOnEventAnimation) {
            Launch();
        }
    }

    private void Launch() {
#if DEBUG_LauncherProjectileComponent
        Debug.Log("Create and launched a projectile with custom velocity: " + useCustomVelocity + ".");
#endif

        //create the proyectile
        GameObject projectile = Instantiate(prefabProjectile);
        projectile.transform.position = transform.position + offsetLauncher;
        ProjectileComponent projectileCmp = projectile.GetComponentInChildren<ProjectileComponent>();
        projectileCmp.SetDirection(direction);
        projectileCmp.LayerCollision = layerCollision;
        if (useCustomVelocity) {
            projectileCmp.Velocity = customVelocityProjectile;
        }
        //get proyectile component and set direction
        GameControllerTemporal.AddTemporal(projectile);
    }
}
