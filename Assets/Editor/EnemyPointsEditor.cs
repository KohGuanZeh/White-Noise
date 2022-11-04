using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyPoints))]
public class EnemyPointsEditor : Editor {
    SerializedProperty points;

    void OnEnable()
    {
        points = serializedObject.FindProperty("points");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        if (EditorGUILayout.LinkButton("Update Points")){
            points.ClearArray();
            for (int i = 0; i < (target as EnemyPoints).transform.childCount; i++){
                points.InsertArrayElementAtIndex(i);
                points.GetArrayElementAtIndex(i).objectReferenceValue = (target as EnemyPoints).transform.GetChild(i);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
