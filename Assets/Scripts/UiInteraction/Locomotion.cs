using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class Locomotion : MonoBehaviour
{
   [SerializeField] protected RectTransform rectTransform;

   [SerializeField] private UIMotion _resetParameters;
      
   public UIMotion[] Motions;
   

   private void Start()
   {
      _resetParameters._dis = rectTransform.position;
   }

   protected void ResetPosition()
   {
      rectTransform.DOAnchorPos( _resetParameters._dis , _resetParameters.timeToTransition).SetEase(_resetParameters.Ease);
   }
}
