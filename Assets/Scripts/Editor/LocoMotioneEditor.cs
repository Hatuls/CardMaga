using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocoMotionUI))]
public class LocoMotioneEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocoMotionUI script = (LocoMotionUI)target;
        
        //if (GUILayout.Button("Update RectTransform"))
          // script.UpdateRectTransform();
            

    }
}
