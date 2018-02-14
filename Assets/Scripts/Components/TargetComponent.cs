//#define DEBUG_TargetComponent

using UnityEngine;
using System;

/// <summary>
/// Component used to keep track of a target and warns if it moved or has changed
/// </summary>
public class TargetComponent : MonoBehaviour {

    /// <summary>
    /// Which distance from its original position we consider a movement
    /// so it warn the classes about a change
    /// </summary>
    public float treshold = 0.5f;
    /// <summary>
    /// The target to keep an eye on
    /// </summary>
    private GameObject targetObject;
    /// <summary>
    /// position of the target last time checked
    /// </summary>
    private Vector3 position;
    /// <summary>
    /// 
    /// </summary>
    private bool init = false;
    /// <summary>
    /// Suscribers for target movement
    /// </summary>
    private Action onTargetChange;
    /// <summary>
    /// Suscribers for target changes
    /// </summary>
    private Action onTargetMove;

    /// <summary>
    /// Check if the target has moved
    /// </summary>
    public void Update() {
        //at first init check the target (not in start because of suscribers)
        if (!init) {
            init = true;
            if (targetObject != null) {
                SetTargetObject(targetObject);
            }
        }

        //if watching a target, check position
        if (targetObject != null) {
            Vector3 newPosition = targetObject.transform.position;
            if (Mathf.Abs(Vector3.Distance(newPosition, position)) > treshold) {
                SetTargetMove();
            }
        }
    }

    /// <summary>
    /// We are tracking a target
    /// </summary>
    /// <returns>return if tracking a target</returns>
    public bool HasTarget() {
        return targetObject != null;
    }

    /// <summary>
    /// A target has moved, store the position and warn suscribers
    /// </summary>
    private void SetTargetMove() {
#if DEBUG_TargetComponent
        Debug.Log("Movement of target.");
#endif
        if (targetObject != null) {
            position = targetObject.transform.position;
        }
        if (onTargetMove!= null) {
            onTargetMove();
        }
    }

    /// <summary>
    /// Set a new target and warn suscribers
    /// </summary>
    /// <param name="newTarget">new target to track</param>
    public void SetTargetObject(GameObject newTarget) {
#if DEBUG_TargetComponent
        Debug.Log("Change target.");
#endif
        targetObject = newTarget;
        if (targetObject != null) {
            position = targetObject.transform.position;
        }
        if (onTargetChange != null) {
            onTargetChange();
        }
    }

    /// <summary>
    /// Get the actual position of the target
    /// </summary>
    /// <returns>actual position of the target or zero if no target</returns>
    public Vector3 GetTargetPosition() {
        return (targetObject != null) ? targetObject.transform.position : Vector3.zero;
    }

    /// <summary>
    /// It return the last position registered for a target
    /// while the target is active, returns the same result as GetTargetPosition
    /// If there is not a target, GetTargetPosition returns zero while this one
    /// return the last position of the previous target we had
    /// (or zero if never had a target)
    /// </summary>
    /// <returns>last position stored of a target</returns>
    public Vector3 GetLastTargetPosition() {
        return position;
    }

    /// <summary>
    /// Register suscribers for target changes
    /// </summary>
    /// <param name="callback">function to be called</param>
    public void RegisterOnTargetChange(Action callback) {
        onTargetChange += callback;
    }

    /// <summary>
    /// Unregister suscribers for target changes
    /// </summary>
    /// <param name="callback">the function to be unregistered</param>
    public void UnregisterOnTargetChange(Action callback) {
        onTargetChange -= callback;
    }

    /// <summary>
    /// Register suscribers for target moves
    /// </summary>
    /// <param name="callback">function to be called</param>
    public void RegisterOnTargetMove(Action callback) {
        onTargetMove += callback;
    }


    /// <summary>
    /// Unregister suscribers for target moves
    /// </summary>
    /// <param name="callback">function to unregister</param>
    public void UnregisterOnTargetMove(Action callback) {
        onTargetMove -= callback;
    }
}
