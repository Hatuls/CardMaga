using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveParametersSO", menuName = "ScriptableObjects/LocoMotion/MoveParametersSO")]
public class LocoMotionSO : ScriptableObject
{
    [Header("Motion Parameters")] 
    
    public float timeToTransition = 1.0f;

    public List<RectTransform> Motions;
    
    public Ease Ease;
    
    public void UpdateRectTransform()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("LocoMotioneRectTransform");
        
        for (int i = 0; i < temp.Length; i++)
        {
            RectTransform tempRect = temp[i].GetComponent<RectTransform>();
            
            if (!Motions.Contains(tempRect))
            {
                Motions.Add(tempRect);
                Debug.Log(Motions[i]);
            }
        }

        Debug.Log(this.name + " RectTransforms update completed");
    }
}
