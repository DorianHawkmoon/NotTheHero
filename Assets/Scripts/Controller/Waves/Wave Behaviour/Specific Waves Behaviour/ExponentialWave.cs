using UnityEngine;
using System;

public class ExponentialWave : WaveBehaviour {
    private ExponentialData data;

    private float timer;
    private int timeCounter;
    private int numberCounter;
    
    private int numberLeft = 0;

    private delegate double Ease(double t, double b, double c, double d);
    private Ease easeCounter;
    private Ease easeTimer;

    public override void SetData(BehaviourData data) {
        this.data = (ExponentialData)data;
        easeCounter = (Ease)Delegate.CreateDelegate(typeof(Ease), typeof(EasingEquationsDouble).GetMethod(this.data.numberFunction.ToString()));
        easeTimer = (Ease)Delegate.CreateDelegate(typeof(Ease), typeof(EasingEquationsDouble).GetMethod(this.data.timeFunction.ToString()));
    }

    public override void StartWaveBehaviour() {
        base.StartWaveBehaviour();
        timeCounter = 0;
        timer = NextTimer();
        Debug.Log("Tiempo a esperar: " + timer);
        numberLeft = data.numberEnemies;

    }

    private void Update() {
        if (!started || finished)  return;
        

        if(!finished && alive<=0 && timeCounter >= data.steps) {
            finished = true;
            return;
        }

        if (timeCounter < data.steps) {
            timer -= Time.deltaTime;
            if (timer < 0) {
                timer = NextTimer();
                Debug.Log("Tiempo a esperar: " + timer);
                SpawnRandomHeroe();
            }
        }
    }

    private void SpawnRandomHeroe() {
        int numberHeroes = NextNumberEnemies();
        Debug.Log("Cuantos enemigos a spawnear: " + numberHeroes);
        for (int i = 0; i < numberHeroes && numberLeft>0; ++i) {
            int heroIndex = UnityEngine.Random.Range(0, data.heroes.Length - 1);
            CreateHero(data.heroes[heroIndex]);
            --numberLeft;
        }
    }

    private float NextTimer() {
        float result = (float)easeTimer(timeCounter, data.beginValueTime, data.endValueTime-data.beginValueTime, data.steps);
        ++timeCounter;
        return result;
    }

    private int NextNumberEnemies() {
        int result = (int)easeCounter(numberCounter, data.initialNumber, data.finalNumber-data.initialNumber, data.steps);
        ++numberCounter;
        return result;
    }

    
}
