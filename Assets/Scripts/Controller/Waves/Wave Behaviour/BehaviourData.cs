using UnityEngine;

public abstract class BehaviourData : ScriptableObject {
    /// <summary>
    /// Available heroes
    /// </summary>
    public GameObject[] heroes;
    /// <summary>
    /// Number of enemies for the wave
    /// </summary>
    public int numberEnemies;

    public abstract TypeWave GetTypeBehaviour();
}
