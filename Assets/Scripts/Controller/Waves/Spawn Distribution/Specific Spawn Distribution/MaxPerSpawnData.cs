using UnityEngine;

[CreateAssetMenu(fileName = "MaximumsDataSpawn", menuName = "Waves/Spawn Distribution/Maximum")]
public class MaxPerSpawnData : SpawnData {
    private TypeSpawnDistribution type = TypeSpawnDistribution.MaxPerSpawn;
    public int[] maximumsPerSpawn;

    public override TypeSpawnDistribution GetTypeSpawn() {
        return type;
    }
}