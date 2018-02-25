using UnityEngine;

public abstract class BehaviourData : ScriptableObject {
    public GameObject[] heroes;

    public abstract TypeWave GetTypeBehaviour();
}
