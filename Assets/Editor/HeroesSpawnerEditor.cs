/*using UnityEditor;

[CustomEditor(typeof(HeroesSpawner))]
public class HeroesSpawnerEditor : Editor {
    // We'll cache the editor here
    private Editor cachedEditor;

    /* using this boolean to keep track if whether cachedEditor has already been assigned too.
     We're required to call Editor.CreateEditor() from OnInspectorGUI(), which is called often, 
     but we only really need to call Editor.CreateEditor() once. * /
    private bool cachedEditorNeedsRefresh = true;

    public void OnEnable() {
        // Resetting cachedEditor, and marking it to be reassigned
        cachedEditor = null;
        cachedEditorNeedsRefresh = true;
    }


    public override void OnInspectorGUI() {
        // Grabbing the object this inspector is editing.
        HeroesSpawner editedMonobehaviour = (HeroesSpawner)target;

        //Checking if we need to get our Editor. Calling Editor.CreateEditor() if needed
        if (cachedEditorNeedsRefresh) {
            cachedEditor = Editor.CreateEditor(editedMonobehaviour.spawnPoints);

            //Ensuring this is only run once.
            cachedEditorNeedsRefresh = false;
        }

        //Drawing our ScriptableObjects inspector
        cachedEditor.DrawDefaultInspector();

        /* We want to show the other variables in our Monobehaviour as well, so we'll call
         the superclasses' OnInspectorGUI(). Note this could also be accomplished by a call
         to DrawDefaultInspector() * /
        base.DrawDefaultInspector();
    }
}*/