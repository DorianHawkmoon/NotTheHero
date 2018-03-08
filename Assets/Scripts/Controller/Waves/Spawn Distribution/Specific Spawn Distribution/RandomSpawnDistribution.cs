using UnityEngine;

public class RandomSpawnDistribution : SpawnDistribution {
    private RandomSpawnData data;

    public override void SetData(SpawnData data) {
        this.data = (RandomSpawnData)data;
    }

    public override void SpawnHero(GameObject hero) {
        int random = Random.Range(0, data.points.spawnPositions.Count);
        Debug.Log("Random: " + random);
        GameControllerTemporal.AddTemporal(hero);
        hero.transform.position = data.points.spawnPositions[random];
    }
}
