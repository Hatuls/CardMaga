using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocoMotionSO))]
public class DropDownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocoMotionSO script = (LocoMotionSO)target;

        GUIContent arrayLabel = new GUIContent("MyArray");
        script._index = EditorGUILayout.Popup(arrayLabel, script._index, script.rectTjransforms);

    }
}
