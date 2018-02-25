using UnityEngine;

[CreateAssetMenu(fileName = "PercentageDataSpawn", menuName = "Waves/Spawn Distribution/Percentage")]
public class PercentageData : SpawnData {
    private TypeSpawnDistribution type = TypeSpawnDistribution.Percentage;
    public float[] percentageOfUse;
    [HideInInspector]
    public float totalPercentage;

    public override TypeSpawnDistribution GetTypeSpawn() {
        return type;
    }
}
