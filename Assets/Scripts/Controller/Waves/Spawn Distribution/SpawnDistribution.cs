using UnityEngine;

public abstract class SpawnDistribution {
    public SpawnPoints spawnPoints;

    public abstract void SetData(SpawnData data);
    public abstract void SpawnHero(GameObject prefab);
}
