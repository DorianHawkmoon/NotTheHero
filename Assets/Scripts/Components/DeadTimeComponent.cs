//#define DEBUG_DeadTimeComponent

using System;
using UnityEngine;

public class DeadTimeComponent : MonoBehaviour {
#if DEBUG_DeadTimeComponent
    private static DebugLog log = new DebugLog("DeadTimeComponent");
#endif

    /// <summary>
    /// Timelife for the object
    /// </summary>
    [SerializeField]
    private float time;

    /// <summary>
    /// If the timer has started
    /// </summary>
    [SerializeField]
    private bool started=true;

    /// <summary>
    /// Timer
    /// </summary>
    private float timer;

    /// <summary>
    /// Suscriber for the end of the object
    /// </summary>
    private Action onEndTime;

    /// <summary>
    /// Set the timer
    /// </summary>
    public void Start() {
        timer = time;
    }

    /// <summary>
    /// Start the timer
    /// </summary>
    public void StartTimer() {
        #if DEBUG_DeadTimeComponent
        log.Log("Timer started.");
        #endif
        timer = time;
        started = true;
    }

    /// <summary>
    /// If the timer has started
    /// </summary>
    /// <returns></returns>
    public bool HasStartedTimer() {
        return started;
    }

    /// <summary>
    /// Check if the timer has started, if has over the time and destroy
    /// </summary>
    public void Update() {
        if (!started) return;

        timer -= Time.deltaTime;
        if (timer < 0 ) {
            #if DEBUG_DeadTimeComponent
            log.Log("Time is over.");
            #endif
            if (onEndTime != null) {
                onEndTime();
            }
            Destroy(gameObject);
        }
    }


    public void RegisterEndTime(Action callback) {
        #if DEBUG_DeadTimeComponent
        log.Log("Registered suscriber for end time.");
        #endif
        onEndTime += callback;
    }

    public void UnregisterOnEndTime(Action callback) {
        #if DEBUG_DeadTimeComponent
        log.Log("Unregistered suscriber for end time.");
        #endif
        onEndTime -= callback;
    }
}
