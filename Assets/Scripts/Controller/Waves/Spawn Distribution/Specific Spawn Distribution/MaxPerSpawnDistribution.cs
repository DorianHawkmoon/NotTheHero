using UnityEngine;

public class MaxPerSpawnDistribution : SpawnDistribution {
    private MaxPerSpawnData data;
    private float counter;

    public override void SetData(SpawnData data) {
        this.data = (MaxPerSpawnData)data;
        counter = 0;
        for(int i=0; i<this.data.maximumsPerSpawn.Length; ++i) {
            counter += this.data.maximumsPerSpawn[i];
        }
    }

    public override void SpawnHero(GameObject hero) {
        int random = Random.Range(0, data.points.spawnPositions.Count);

        //if max number achieved in this point, get next
        //if all of them has achieved maximum (it should not be posible), just get random
        while (data.maximumsPerSpawn[random] <= 0 && counter>0) {
            random = Random.Range(0, data.points.spawnPositions.Count);
        }

        GameControllerTemporal.AddTemporal(hero);
        hero.transform.position = data.points.spawnPositions[random];
        data.maximumsPerSpawn[random]-=1;
        counter--;
    }

}
