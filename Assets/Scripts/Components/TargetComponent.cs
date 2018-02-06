using UnityEngine;using System;public class TargetComponent : MonoBehaviour {    public GameObject targetObject;//TODO change to private
    public float treshold=0.5f;

    private Vector3 position;    private bool init = false;    private Action onTargetChange;    public void Update() {
        if (!init) {
            init = true;
            if (targetObject != null) {
                SetTargetObject(targetObject);
            }
        }

        Vector3 newPosition = targetObject.transform.position;
        if (Mathf.Abs(Vector3.Distance(newPosition, position)) > treshold) {
            SetTargetObject(targetObject);
        }
    }    public void SetTargetObject(GameObject newTarget) {
        targetObject = newTarget;
        position = targetObject.transform.position;
        onTargetChange();
    }    public Vector3 GetTargetPosition() {        return targetObject.transform.position;    }    public void RegisterOnTargetChange(Action callback) {
        onTargetChange += callback;
    }    public void UnregisterOnTargetChange(Action callback) {
        onTargetChange -= callback;
    }}