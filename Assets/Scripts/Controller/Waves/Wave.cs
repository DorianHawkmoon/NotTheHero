using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A wave has definition about how it behaves, how the spawns are distributed around the points
/// and a list of personals events during the wave
/// </summary>
///[Serializable]
[CreateAssetMenu(fileName ="Wave", menuName="Waves/Wave")]
public class Wave : ScriptableObject {
    public SpawnPoints spawnPoints;

    public SpawnData spawnData;
    public BehaviourData behaviourData;

    private WaveBehaviour waveBehaviour=null;
    private SpawnDistribution spawnDistribution=null;

    public List<EventWave> waveEvents;

    public virtual void StartWave(GameObject waveInstance) {
        waveBehaviour = FactoryWaveBehaviour.CreateWaveBehaviour(behaviourData.GetTypeBehaviour(), behaviourData, waveInstance);
        spawnDistribution = FactorySpawnDistribution.CreateSpawnDistribution(spawnData.GetTypeSpawn(), spawnData);

        waveBehaviour.AddPrefabSpawner(SpawnHero);
        waveBehaviour.StartWaveBehaviour();

        foreach (EventWave eventWave in waveEvents) {
            eventWave.StartEvent();
        }
    }

    public virtual void UpdateWave() {
        foreach (EventWave eventWave in waveEvents) {
            eventWave.StartEvent();
        }
    }

    /// <summary>
    /// When the wave is over
    /// </summary>
    /// <returns></returns>
    public bool WaveOver() {
        //TODO need a way to check if the behaviour has finished, but not the list of events
        return waveBehaviour.IsFinished();
    }

    /// <summary>
    /// It's time to spawn a hero, tell the spawner to do it
    /// </summary>
    /// <param name="hero"></param>
    private void SpawnHero(GameObject hero) {
        spawnDistribution.SpawnHero(hero);
    }
}
