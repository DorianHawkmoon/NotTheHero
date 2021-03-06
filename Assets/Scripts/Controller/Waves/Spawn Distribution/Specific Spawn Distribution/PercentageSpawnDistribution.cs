﻿//#define DEBUG_PercentageSpawnDistribution

using UnityEngine;

public class PercentageSpawnDistribution : SpawnDistribution {
    private PercentageData data;

    public override void SetData(SpawnData data) {
        this.data = (PercentageData)data;
    }

    public override void SpawnHero(GameObject hero) {
        float random = Random.Range(0.0f, data.totalPercentage);
        #if DEBUG_PercentageSpawnDistribution
        Debug.Log("Random: " + random);
        #endif
        float sum = 0;
        int i = 0;
        while (sum <= random) {
            sum += data.percentageOfUse[i];
            ++i;
        }
        #if DEBUG_PercentageSpawnDistribution
        Debug.Log("Index: " + (i-1));
        #endif
        GameControllerTemporal.AddTemporal(hero);
        hero.transform.position = data.points.spawnPositions[i-1];
    }
}
