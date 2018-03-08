using UnityEditor;

[CustomEditor(typeof(PercentageData))]
[CanEditMultipleObjects]
public class PercentageDataEditor : Editor {
    private Editor cachedEditor;
    private bool cachedEditorNeedsRefresh = true;

    public void OnEnable() {
        // Resetting cachedEditor, and marking it to be reassigned
        cachedEditor = null;
        cachedEditorNeedsRefresh = true;
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();

        PercentageData pd = (PercentageData)target;

        if (pd.points != null) {
            int numberSpawns = pd.points.spawnPositions.Count;
            if (pd.percentageOfUse.Length != numberSpawns) {
                pd.percentageOfUse = new float[numberSpawns];
            }
        }

        //Checking if we need to get our Editor. Calling Editor.CreateEditor() if needed
        if (cachedEditorNeedsRefresh) {
            cachedEditor = Editor.CreateEditor(pd);

            //Ensuring this is only run once.
            cachedEditorNeedsRefresh = false;
        }

        cachedEditor.DrawDefaultInspector();

        float total = 0;
        for(int i=0; i<pd.percentageOfUse.Length; ++i) {
            total += pd.percentageOfUse[i];
        }
        pd.totalPercentage = total;
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Total percentage: " + pd.totalPercentage);
        EditorGUI.indentLevel--;

        EditorGUI.EndChangeCheck();
    }
}