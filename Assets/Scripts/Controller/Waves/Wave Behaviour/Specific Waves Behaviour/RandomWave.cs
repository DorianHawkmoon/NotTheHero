using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RandomWave : WaveBehaviour {
    private RandomData data;

    private int numberLeft=0;
    private float timer;

    public void SetData(BehaviourData data) {
        this.data = (RandomData)data;
    }

    
    public override void StartWaveBehaviour() {
        base.StartWaveBehaviour();
        timer = Random.Range(data.minRandomSpawn, data.maxRandomSpawn);
        numberLeft = data.numberEnemies;
    }

    private void Update() {
        if (!started) return;
        timer -= Time.deltaTime;
        if (timer<0 && numberLeft>0) {
            SpawnRandomHeroe();
            timer = Random.Range(data.minRandomSpawn, data.maxRandomSpawn);
        }
    }

    private void SpawnRandomHeroe() {
        int heroIndex = Random.Range(0, data.heroes.Length - 1);
        GameObject hero = Instantiate(data.heroes[heroIndex]);
        prefabSpawners(hero);
        --numberLeft;
    }
}
