using UnityEditor;
using System;
using UnityEngine;

[CustomEditor(typeof(MaxPerSpawnData))]
[CanEditMultipleObjects]
public class MaxPerSpawnDataEditor : Editor {
    private Editor cachedEditor;
    private bool cachedEditorNeedsRefresh = true;

    public void OnEnable() {
        // Resetting cachedEditor, and marking it to be reassigned
        cachedEditor = null;
        cachedEditorNeedsRefresh = true;
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();

        MaxPerSpawnData pd = (MaxPerSpawnData)target;

        if (pd.points != null) {
            int numberSpawns = pd.points.spawnPositions.Count;
            if (pd.maximumsPerSpawn.Length != numberSpawns) {
                pd.maximumsPerSpawn = new int[numberSpawns];
            }
        }

        //Checking if we need to get our Editor. Calling Editor.CreateEditor() if needed
        if (cachedEditorNeedsRefresh) {
            cachedEditor = Editor.CreateEditor(pd);

            //Ensuring this is only run once.
            cachedEditorNeedsRefresh = false;
        }

        cachedEditor.DrawDefaultInspector();

        int total = 0;
        for (int i = 0; i < pd.maximumsPerSpawn.Length; ++i) {
            total += pd.maximumsPerSpawn[i];
        }
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Total to spawn: " + total);
        EditorGUI.indentLevel--;

        EditorGUI.EndChangeCheck();
    }
}