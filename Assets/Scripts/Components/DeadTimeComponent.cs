using System;
using UnityEngine;

public class DeadTimeComponent : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float time;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private bool started=true;
    private float timer;

    private Action onEndTime;

    public void Start() {
        timer = time;
    }

    public void StartTimer() {
        timer = time;
        started = true;
    }

    public bool HasStartedTimer() {
        return started;
    }

    public void Update() {
        if (!started) return;

        timer -= Time.deltaTime;
        if (timer < 0 ) {
            if (onEndTime != null) {
                onEndTime();
            }
            Destroy(gameObject);
        }
    }

    public void RegisterEndTime(Action callback) {
        onEndTime += callback;
    }

    public void UnregisterOnEndTime(Action callback) {
        onEndTime -= callback;
    }
}
