//#define DEBUG_PathFinderComponent

using System;
using UnityEngine;

/// <summary>
/// Given a target, look for a path to it
/// </summary>
public class PathFinderComponent : MonoBehaviour {
#if DEBUG_PathFinderComponent
    private static DebugLog log = new DebugLog("PathFinderComponent");
#endif

    /// <summary>
    /// Target component who tell the destination
    /// </summary>
    private TargetComponent target;
    /// <summary>
    /// Path made of keypoints
    /// </summary>
    private Vector3[] path;
    /// <summary>
    /// Suscribers who wants to know when the path has changed
    /// </summary>
    private Action onPathChange;

    /// <summary>
    /// Get needed components
    /// </summary>
    public void Start() {
        target = GetComponent<TargetComponent>();
        Debug.Assert(target != null);
        target.RegisterOnTargetChange(OnTargetChanged);
        target.RegisterOnTargetMove(OnTargetChanged);
    }

    /// <summary>
    /// When the target changes, get a new path
    /// </summary>
    public void OnTargetChanged() {
        #if DEBUG_PathFinderComponent
        log.Log("Target changed.");
        #endif

        if (target.HasTarget()) {
            #if DEBUG_PathFinderComponent
            log.Log("Requesting new path.");
            #endif
            PathRequestManager.RequestPath(transform.position, target.GetTargetPosition(), OnPathFound);
        } else {
            #if DEBUG_PathFinderComponent
            log.Log("There is no target.");
            #endif
            path = null;
            onPathChange();
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        #if DEBUG_PathFinderComponent
        log.Log("Achieved path: "+pathSuccessful+".");
        #endif

        if (pathSuccessful) {
            path = newPath;
            onPathChange();
        }
    }

    public Vector3[] GetPath() {
        return path;
    }

    public void RegisterOnPathChange(Action callback) {
        #if DEBUG_PathFinderComponent
        log.Log("Suscriber added to path change.");
        #endif
        onPathChange += callback;
    }

    public void UnregisterOnPathChange(Action callback) {
        #if DEBUG_PathFinderComponent
        log.Log("Suscriber deleted to path change.");
        #endif
        onPathChange -= callback;
    }

}
