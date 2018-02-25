using UnityEngine;

public abstract class SpawnData : ScriptableObject {
    [HideInInspector]
    public SpawnPoints points;

    public abstract TypeSpawnDistribution GetTypeSpawn();
}
