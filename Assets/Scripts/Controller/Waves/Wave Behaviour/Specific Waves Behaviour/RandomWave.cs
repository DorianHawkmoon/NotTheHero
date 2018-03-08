using UnityEngine;

public class RandomWave : WaveBehaviour {
    private RandomData data;

    private int numberLeft=0;
    private float timer;

    public override void SetData(BehaviourData data) {
        this.data = (RandomData)data;
    }

    
    public override void StartWaveBehaviour() {
        base.StartWaveBehaviour();
        timer = Random.Range(data.minRandomSpawn, data.maxRandomSpawn);
        numberLeft = data.numberEnemies;
    }

    private void Update() {
        if (!started || numberLeft<=0) return;
        timer -= Time.deltaTime;
        if (timer<0) {
            SpawnRandomHeroe();
            timer = Random.Range(data.minRandomSpawn, data.maxRandomSpawn);
        }
    }

    private void SpawnRandomHeroe() {
        int heroIndex = Random.Range(0, data.heroes.Length - 1);
        CreateHero(data.heroes[heroIndex]);
        --numberLeft;
    }

    protected override void DeathHero() {
        base.DeathHero();
        if (numberLeft <= 0 && alive <= 0) {
            finished = true;
        }
    }
}
