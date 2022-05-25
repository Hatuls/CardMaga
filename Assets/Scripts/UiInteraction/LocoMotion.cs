using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class LocoMotion : MonoBehaviour
{
   [SerializeField] private TouchableItem _touchableItem; //test
   
   [SerializeField] protected RectTransform rectTransform;

   [SerializeField] private LocoMotionSO _resetParameters;
      
   public LocoMotionSO[] Motions;

   private void Start()
   {
      Debug.Log("Start");
      _touchableItem.OnClick += Move;
   }

   public void Move()
   {
      Debug.Log("Move");
   }
}
