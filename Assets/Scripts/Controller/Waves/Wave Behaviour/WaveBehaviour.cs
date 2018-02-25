using System;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {
    protected bool started = false;

    protected Action<GameObject> prefabSpawners;

    public virtual void StartWaveBehaviour() {
        started = true;
    }

    public void AddPrefabSpawner(Action<GameObject> listener) {
        prefabSpawners += listener;
    }

    public void RemovePrefabSpawner(Action<GameObject> listener) {
        prefabSpawners -= listener;
    }
}
