using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Wave))]
[CanEditMultipleObjects]
public class WaveEditor : Editor {
    // We'll cache the editor here
    private Editor cachedEditorPoints;
    private Editor cachedEditorSpawn;
    private Editor cachedEditorBehaviour;

    /* using this boolean to keep track if whether cachedEditor has already been assigned too.
     We're required to call Editor.CreateEditor() from OnInspectorGUI(), which is called often, 
     but we only really need to call Editor.CreateEditor() once. */
    private bool cachedEditorPointsNeedsRefresh = true;
    private bool cachedEditorSpawnNeedsRefresh = true;
    private bool cachedEditorBehaviourNeedsRefresh = true;

    public void OnEnable() {
        // Resetting cachedEditor, and marking it to be reassigned
        cachedEditorSpawn = null;
        cachedEditorBehaviour = null;
        cachedEditorPoints = null;

        cachedEditorSpawnNeedsRefresh = true;
        cachedEditorBehaviourNeedsRefresh = true;
        cachedEditorPointsNeedsRefresh = true;
    }


    public override void OnInspectorGUI() {
        // Grabbing the object this inspector is editing.
        Wave wave = (Wave)target;

        if (cachedEditorPointsNeedsRefresh) {
            cachedEditorPoints = Editor.CreateEditor(wave.spawnPoints);
            cachedEditorPointsNeedsRefresh = false;
        }
        
        if (cachedEditorBehaviourNeedsRefresh){
            cachedEditorBehaviour = Editor.CreateEditor(wave.behaviourData);
            cachedEditorBehaviourNeedsRefresh = false;
        }

        if (cachedEditorSpawnNeedsRefresh) {
            cachedEditorSpawn = Editor.CreateEditor(wave.spawnData);
            if (wave.spawnData != null) {
                wave.spawnData.points = wave.spawnPoints;
            }
            cachedEditorSpawnNeedsRefresh = false;
        }

        if (base.DrawDefaultInspector()) {
            UpdateData();
            EditorUtility.SetDirty(target);
        }

        if (cachedEditorPoints != null) {
            EditorGUILayout.Space();
            cachedEditorPoints.DrawDefaultInspector();
        }

        if (cachedEditorBehaviour != null) {
            EditorGUILayout.Space();
            
            GUI.backgroundColor = Color.cyan;
            cachedEditorBehaviour.DrawDefaultInspector();
        }

        if (cachedEditorSpawn != null) {
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.green;
            cachedEditorSpawn.OnInspectorGUI();
        }
    }

    private void UpdateData() {
        cachedEditorBehaviourNeedsRefresh = true;
        cachedEditorPointsNeedsRefresh = true;
        cachedEditorSpawnNeedsRefresh = true;
    }

}
