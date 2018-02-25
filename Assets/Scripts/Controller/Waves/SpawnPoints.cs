using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn points", menuName = "Spawn points level")]
public class SpawnPoints : ScriptableObject {

    public List<Vector3> spawnPositions;

    public void OnDrawGizmos() {
        Gizmos.color = Color.cyan;

        int i = 0;
        foreach (Vector3 child in spawnPositions) {
            //Transform transform = child.transform;
            TextGizmo.Draw(child, "Spawner " + i);
            Gizmos.DrawSphere(child, 0.3f);
            ++i;
        }
    }
}
