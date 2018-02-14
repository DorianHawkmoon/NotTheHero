//#define DEBUG_GameControllerTemporal

using UnityEngine;
//TODO allow diferents temporal parents (maybe other class and for example heroes)
/// <summary>
/// Game Controller's component used to organize all the temporal
/// objects created on runtime which are not parented to any
/// object (like particle systems).
/// </summary>
public class GameControllerTemporal : MonoBehaviour {

    /// <summary>
    /// The name of the temporal parent. If this object doesn't exist,
    /// it will be created.
    /// </summary>
    public string temporalObjectName = "Temporal Objects";

    /// <summary>
    /// Reference to the temporal parent.
    /// </summary>
    private static Transform temporalParent;

    void Awake() {
        // Creates the temporal parent
        CreateTemporalParent();
#if DEBUG_GameControllerTemporal
        Debug.Log("Temporal folder generated.");
#endif
    }

    /// <summary>
    /// Returns the scene's temporal objects' parent.
    /// </summary>
    /// <returns>The temporal objects' parent</returns>
    public static Transform GetTemporalParent() {
        return temporalParent;
    }

    /// <summary>
    /// Registers a temporal object.
    /// </summary>
    /// <param name="temporalObject">The temporal object to register</param>
    public static void AddTemporal(GameObject temporalObject) {
        AddTemporal(temporalObject.transform);
    }

    /// <summary>
    /// Registers a temporal object.
    /// </summary>
    /// <param name="temporalObject">The temporal object to register</param>
    public static void AddTemporal(Transform temporalObject) {
#if DEBUG_GameControllerTemporal
        Debug.Log("Temporal object added.");
#endif
        temporalObject.parent = temporalParent;
    }

    /// <summary>
    /// Looks for the temporal objects' parent. It it doesn't exist, creates it.
    /// </summary>
    private void CreateTemporalParent() {
        GameObject temporalParent = GameObject.Find(temporalObjectName);
        if (temporalParent != null)
            GameControllerTemporal.temporalParent = temporalParent.transform;
        else
            GameControllerTemporal.temporalParent = new GameObject(temporalObjectName).transform;
    }
}