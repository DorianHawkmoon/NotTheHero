using UnityEngine;

public class ConstantNumberWaves : WaveBehaviour {
    private ConstantNumberData data;

    private int numberLeft = 0;

    public override void SetData(BehaviourData data) {
        this.data = (ConstantNumberData)data;
    }

    public override void StartWaveBehaviour() {
        base.StartWaveBehaviour();
        numberLeft = data.numberEnemies;
    }

    private void Update() {
        if (started && !finished && numberLeft>0 && alive < data.constantNumberWaves) {
            SpawnRandomHeroe();
        }
    }

    private void SpawnRandomHeroe() {
        int heroIndex = Random.Range(0, data.heroes.Length - 1);
        CreateHero(data.heroes[heroIndex]);
        
        --numberLeft;
    }

    protected override void DeathHero() {
        base.DeathHero();
        if(alive<=0 && numberLeft <= 0) {
            finished = true;
        }
    }
}
