//#define DEBUG_LifeComponent

using System;
using UnityEngine;

/// <summary>
/// Class for the life of characters or items
/// </summary>
public class LifeComponent : MonoBehaviour {

    /// <summary>
    /// Life of the entity
    /// </summary>
    [SerializeField]
    private int life;

    /// <summary>
    /// Has died?
    /// </summary>
    private bool died = false;

    /// <summary>
    /// Suscriptor for death
    /// </summary>
    private Action onDeath;

    /// <summary>
    /// Suscriptor for changes in life
    /// </summary>
    private Action<int> onLifeChange;

    /// <summary>
    /// Made damage (or health)
    /// Negative amounts health life
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage) {
        #if DEBUG_LifeComponent
        Debug.Log("Damage: " + damage + ".");
        #endif

        life -= damage;

        if (onLifeChange != null) {
            onLifeChange(life);
        }

        if (life <= 0 && !died) {
            died = true;
            if (onDeath != null) {
                onDeath();
                Die();
            }
        }
    }

    /// <summary>
    /// Is dead the character/item?
    /// </summary>
    /// <returns>true if it's dead</returns>
    public bool IsDead() {
        return died;
    }

    /// <summary>
    /// When the character dies, init an animation and remove the object
    /// </summary>
    private void Die() {
        #if DEBUG_LifeComponent
        Debug.Log("Die.");
        #endif
        //TODO
        //init animation Die
        //else, remove gameobject
    }

    /// <summary>
    /// Event when animation of ending has finished
    /// </summary>
    private void AnimationDied() {
        //TODO
    }

    /// <summary>
    /// Register suscriber for death
    /// </summary>
    /// <param name="callback"></param>
    public void RegisterOnDeath(Action callback) {
        #if DEBUG_LifeComponent
        Debug.Log("Register on death.");
        #endif
        onDeath += callback;
    }

    /// <summary>
    /// Unregister suscriber for death
    /// </summary>
    /// <param name="callback"></param>
    public void UnregisterOnDeath(Action callback) {
        #if DEBUG_LifeComponent
        Debug.Log("Unregister on death.");
        #endif
        onDeath -= callback;
    }

    /// <summary>
    /// Register suscriber for changes in life
    /// </summary>
    /// <param name="callback"></param>
    public void RegisterOnLifeChange(Action<int> callback) {
        #if DEBUG_LifeComponent
        Debug.Log("Register on life change.");
        #endif
        onLifeChange += callback;
    }

    /// <summary>
    /// Unregister suscriber for changes in life
    /// </summary>
    /// <param name="callback"></param>
    public void UnregisterOnLifeChange(Action<int> callback) {
        #if DEBUG_LifeComponent
        Debug.Log("Unregister on life change.");
        #endif
        onLifeChange -= callback;
    }
}
