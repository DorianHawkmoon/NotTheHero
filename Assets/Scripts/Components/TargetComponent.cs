using UnityEngine;
using System;

public class TargetComponent : MonoBehaviour {
    
    /// <summary>
    /// Which distance from its original position we consider a movement
    /// so it warn the classes about a change
    /// </summary>
    public float treshold = 0.5f;
    private GameObject targetObject;
    private Vector3 position;
    private bool init = false;
    private Action onTargetChange;
    private Action onTargetMove;

    public void Update() {
        if (!init) {
            init = true;
            if (targetObject != null) {
                SetTargetObject(targetObject);
            }
        }

        if (targetObject != null) {
            Vector3 newPosition = targetObject.transform.position;
            if (Mathf.Abs(Vector3.Distance(newPosition, position)) > treshold) {
                SetTargetMove();
            }
        }
    }

    public bool HasTarget() {
        return targetObject != null;
    }

    private void SetTargetMove() {
        if (targetObject != null) {
            position = targetObject.transform.position;
        }
        if (onTargetMove!= null) {
            onTargetMove();
        }
    }

    public void SetTargetObject(GameObject newTarget) {
        targetObject = newTarget;
        if (targetObject != null) {
            position = targetObject.transform.position;
        }
        if (onTargetChange != null) {
            onTargetChange();
        }
    }

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
    /// <returns></returns>
    public Vector3 GetLastTargetPosition() {
        return position;
    }

    public void RegisterOnTargetChange(Action callback) {
        onTargetChange += callback;
    }

    public void UnregisterOnTargetChange(Action callback) {
        onTargetChange -= callback;
    }

    public void RegisterOnTargetMove(Action callback) {
        onTargetMove += callback;
    }

    public void UnregisterOnTargetMove(Action callback) {
        onTargetMove -= callback;
    }
}
