using UnityEditor;

[CustomEditor(typeof(OverlapItem))]
[CanEditMultipleObjects]
public class OverlapItemEditor : Editor {
    SerializedProperty centerOffset;

    SerializedProperty isSphere;
    SerializedProperty isBox;
    SerializedProperty radius;
    SerializedProperty sizeBox;
    
    SerializedProperty radiusExtended;
    SerializedProperty sizeBoxExtended;

    SerializedProperty layer;
    SerializedProperty layerExtended;

    void OnEnable() {
        // Setup the SerializedProperties.
        centerOffset = serializedObject.FindProperty("centerOffset");
        isSphere = serializedObject.FindProperty("isSphere");
        isBox = serializedObject.FindProperty("isBox");
        radius = serializedObject.FindProperty("radius");
        sizeBox = serializedObject.FindProperty("sizeBox");
        radiusExtended = serializedObject.FindProperty("radiusExtended");
        sizeBoxExtended = serializedObject.FindProperty("sizeBoxExtended");
        layer = serializedObject.FindProperty("layerColliders");
        layerExtended = serializedObject.FindProperty("layerCollidersExtended");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        centerOffset.vector3Value = EditorGUILayout.Vector3Field("Center offset", centerOffset.vector3Value);
        
        EditorGUILayout.PropertyField(layer, true);
        EditorGUILayout.PropertyField(layerExtended, true);
        
        EditorGUILayout.Space();

        isSphere.boolValue = !isBox.boolValue;

        EditorGUILayout.PropertyField(isSphere);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(isBox);
        if (EditorGUI.EndChangeCheck()) {
            isSphere.boolValue = !isBox.boolValue;
        }

        if (isSphere.boolValue) {
            isBox.boolValue = false;
            EditorGUILayout.PropertyField(radius);
            EditorGUILayout.PropertyField(radiusExtended);
        }
        
        if (isBox.boolValue) {
            EditorGUILayout.PropertyField(sizeBox);
            EditorGUILayout.PropertyField(sizeBoxExtended);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
