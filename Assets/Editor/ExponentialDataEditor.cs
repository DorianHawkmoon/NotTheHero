using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExponentialData))]
[CanEditMultipleObjects]
public class ExponentialDataEditor : Editor {

    public override void OnInspectorGUI() {
        ExponentialData pd = (ExponentialData)target;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Important: modify in its scriptable object, not in wave.");

        pd.steps = EditorGUILayout.IntField("Steps of wave", pd.steps);
        pd.beginValueTime = EditorGUILayout.FloatField("First time value", pd.beginValueTime);
        pd.endValueTime= EditorGUILayout.FloatField("Last time value", pd.endValueTime);
        pd.timeFunction = (EasingEquationsDouble.EasingEquations)EditorGUILayout.EnumPopup(pd.timeFunction);

        bool changed = EditorGUI.EndChangeCheck();

        if(changed) {
            //recalculate curve
            pd.curveTime=DataEasingToAnimation.ConvertEaseEquation(pd.timeFunction, pd.curveTime);
        }

        EditorGUILayout.CurveField(pd.curveTime);


        EditorGUI.BeginChangeCheck();
        pd.initialNumber = EditorGUILayout.IntField("Start enemies", pd.initialNumber);
        pd.finalNumber = EditorGUILayout.IntField("End enemies", pd.finalNumber);
        pd.numberFunction = (EasingEquationsDouble.EasingEquations)EditorGUILayout.EnumPopup(pd.numberFunction);

        changed = EditorGUI.EndChangeCheck();

        if (changed) {
            //recalculate curve
            pd.curveNumber = DataEasingToAnimation.ConvertEaseEquation(pd.numberFunction, pd.curveNumber);
        }

        EditorGUILayout.CurveField(pd.curveNumber);
    }
}