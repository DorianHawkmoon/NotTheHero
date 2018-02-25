using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPerSpawnDistribution : SpawnDistribution {
    private MaxPerSpawnData data;

    public MaxPerSpawnDistribution(SpawnData data) {
        this.data = (MaxPerSpawnData)data;
    }
    
}
