using UnityEngine;

[CreateAssetMenu(fileName = "RandomDataSpawn", menuName = "Waves/Spawn Distribution/Random")]
public class RandomSpawnData : SpawnData {
    private TypeSpawnDistribution type = TypeSpawnDistribution.Random;

    public override TypeSpawnDistribution GetTypeSpawn() {
        return type;
    }
}
