
public class FactorySpawnDistribution {
    public static SpawnDistribution CreateSpawnDistribution(TypeSpawnDistribution typeSpawn, SpawnData data) {
        SpawnDistribution spawn = null;
        switch (typeSpawn) {
            case TypeSpawnDistribution.MaxPerSpawn:
                spawn = new MaxPerSpawnDistribution();
                break;
            case TypeSpawnDistribution.Percentage:
                spawn = new PercentageSpawnDistribution();
                break;
            case TypeSpawnDistribution.Random:
                spawn = new RandomSpawnDistribution();
                break;
        }
        spawn.SetData(data);
        return spawn;
    }    
}
