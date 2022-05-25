using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocoMotion))]
public class LocoMotioneEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocoMotion script = (LocoMotion)target;
        
        if (GUILayout.Button("Update RectTransform"))
           script.UpdateRectTransform();
            

    }
}
