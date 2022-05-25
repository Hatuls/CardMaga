using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class LocoMotion : MonoBehaviour
{
   [SerializeField] protected RectTransform rectTransform;

   [SerializeField] private LocoMotionSO _resetParameters;
      
   public LocoMotionSO[] Motions;
   
}
