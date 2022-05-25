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
        
        //if (GUILayout.Button("Update RectTransform"))
           // script.UpdateRectTransform();
            

    }
}
