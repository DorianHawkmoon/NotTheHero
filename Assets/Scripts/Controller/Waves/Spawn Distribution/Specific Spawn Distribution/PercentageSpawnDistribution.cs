using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageSpawnDistribution : SpawnDistribution {
    private PercentageData data;

    public PercentageSpawnDistribution(SpawnData data) {
        this.data = (PercentageData)data;
    }

    public override void SpawnHero(GameObject hero) {
        float random = Random.Range(0.0f, data.totalPercentage);
        Debug.Log("Random: " + random);
        float sum = 0;
        int i = 0;
        while (sum <= random) {
            sum += data.percentageOfUse[i];
            ++i;
        }

        Debug.Log("Index: " + (i-1));
        GameControllerTemporal.AddTemporal(hero);
        hero.transform.position = data.points.spawnPositions[i-1];
    }
}
