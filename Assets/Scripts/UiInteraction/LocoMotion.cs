using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class LocoMotion : MonoBehaviour
{
   [SerializeField] protected RectTransform rectTransform;

   [SerializeField] private LocoMotionSO _resetParameters;
   
   [SerializeField] protected List<RectTransform> _motions;
   
   public void UpdateRectTransform()
   {
      GameObject[] temp = GameObject.FindGameObjectsWithTag("LocoMotioneRectTransform");
        
      for (int i = 0; i < temp.Length; i++)
      {
         RectTransform tempRect = temp[i].GetComponent<RectTransform>();
            
         if (!_motions.Contains(tempRect))
         {
            _motions.Add(tempRect);
            Debug.Log(_motions[i]);
         }
      }

      Debug.Log(this.name + " RectTransforms update completed");
   }
   
}
