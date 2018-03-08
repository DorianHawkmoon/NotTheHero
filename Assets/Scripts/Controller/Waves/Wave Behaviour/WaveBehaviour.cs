using System;
using UnityEngine;

public abstract class WaveBehaviour : MonoBehaviour {
    protected bool started = false;
    protected bool finished = false;

    protected int alive;

    protected Action<GameObject> prefabSpawners;

    public abstract void SetData(BehaviourData data);

    public virtual void StartWaveBehaviour() {
        started = true;
        alive = 0;
    }

    public bool IsFinished() {
        return finished;
    }

    public void AddPrefabSpawner(Action<GameObject> listener) {
        prefabSpawners += listener;
    }

    public void RemovePrefabSpawner(Action<GameObject> listener) {
        prefabSpawners -= listener;
    }

    protected GameObject CreateHero(GameObject prefab) {
        GameObject hero = Instantiate(prefab);
        LifeComponent life = hero.GetComponent<LifeComponent>();
        life.RegisterOnDeath(DeathHero);
        prefabSpawners(hero);
        ++alive;
        return hero;
    }

    protected virtual void DeathHero() {
        --alive;
    }
}
