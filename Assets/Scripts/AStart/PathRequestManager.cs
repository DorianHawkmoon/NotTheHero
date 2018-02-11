using System;using UnityEngine;using System.Collections.Generic;/// <summary>
/// Class interface for queue paths petitions
/// </summary>public class PathRequestManager : MonoBehaviour {    /// <summary>
    /// Queue for request
    /// </summary>    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();    /// <summary>
    /// Actual request being procesed
    /// </summary>    PathRequest currentPathRequest;    /// <summary>
    /// unique instance for application
    /// </summary>    static PathRequestManager instance;    /// <summary>
    /// The actual path finder
    /// </summary>    PathFinding pathFinding;    /// <summary>
    /// Is it processing a path?
    /// </summary>    bool isProcessingPath;    /// <summary>
    /// Get needed components
    /// </summary>    public void Awake() {        instance = this;        pathFinding = GetComponent<PathFinding>();    }    /// <summary>
    /// Make a request and store in queu
    /// </summary>
    /// <param name="pathStart"></param>
    /// <param name="pathEnd"></param>
    /// <param name="callback"></param>    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);        instance.pathRequestQueue.Enqueue(newRequest);        instance.TryProcessNext();    }    /// <summary>
    /// Proccess requests
    /// </summary>    private void TryProcessNext() {        if(!isProcessingPath && pathRequestQueue.Count > 0) {            currentPathRequest = pathRequestQueue.Dequeue();            isProcessingPath = true;            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);        }    }    /// <summary>
    /// Has finished processing a request, it call the requester back with the path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="success"></param>    public void FinishedProcessingPath(Vector3[] path, bool success) {        currentPathRequest.callback(path, success);        isProcessingPath = false;        TryProcessNext();    }    /// <summary>
    /// Struct for the request
    /// </summary>    struct PathRequest {        public Vector3 pathStart;        public Vector3 pathEnd;        public Action<Vector3[], bool> callback;        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback) {            pathStart = start;            pathEnd = end;            this.callback = callback;        }    }}