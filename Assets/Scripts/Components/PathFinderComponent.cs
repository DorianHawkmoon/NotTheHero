using System;
using UnityEngine;

public class PathFinderComponent : MonoBehaviour {

    private TargetComponent target;
    private Vector3[] path;
    private Action onPathChange;

    public void Start() {
        target = GetComponent<TargetComponent>();
        target.RegisterOnTargetChange(OnTargetChanged);
    }

    private void OnTargetChanged() {
        PathRequestManager.RequestPath(transform.position, target.GetTargetPosition(), OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            onPathChange();
        }
    }

    public Vector3[] GetPath() {
        return path;
    }

    public void RegisterOnPathChange(Action callback) {
        onPathChange += callback;
    }    public void UnregisterOnPathChange(Action callback) {
        onPathChange -= callback;
    }

    //TODO do the request to the path request manager
    //store the path and keep control of the changes of the target

    //public List<Node> aPath;

}
