﻿using System;
using UnityEngine;

public class PathFinderComponent : MonoBehaviour {

    private TargetComponent target;
    private Vector3[] path;
    private Action onPathChange;

    public void Start() {
        target = GetComponent<TargetComponent>();
        target.RegisterOnTargetChange(OnTargetChanged);
        target.RegisterOnTargetMove(OnTargetChanged);
    }

    public void OnTargetChanged() {
        if (target.HasTarget()) {
            PathRequestManager.RequestPath(transform.position, target.GetTargetPosition(), OnPathFound);
        } else {
            path = null;
            onPathChange();
        }
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
    }

    public void UnregisterOnPathChange(Action callback) {
        onPathChange -= callback;
    }

}
