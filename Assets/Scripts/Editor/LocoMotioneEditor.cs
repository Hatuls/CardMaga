using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocoMotionSO))]
public class LocoMotioneEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocoMotionSO script = (LocoMotionSO)target;

        GUIContent arrayLabel = new GUIContent("MovePos");
        //script._index = EditorGUILayout.Popup(script._index, script.Direction.ToArray().ToString());
        
        if (GUILayout.Button("Update RectTransform")){}
            

    }
}
