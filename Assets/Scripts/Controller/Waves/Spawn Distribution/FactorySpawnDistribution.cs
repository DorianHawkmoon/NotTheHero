
public class FactorySpawnDistribution {
    public static SpawnDistribution CreateSpawnDistribution(TypeSpawnDistribution typeSpawn, SpawnData data) {
        SpawnDistribution spawn = null;
        switch (typeSpawn) {
            case TypeSpawnDistribution.MaxPerSpawn:
                spawn = new MaxPerSpawnDistribution(data);
                break;
            case TypeSpawnDistribution.Percentage:
                spawn = new PercentageSpawnDistribution(data);
                break;
        }
        return spawn;
    }    
}
