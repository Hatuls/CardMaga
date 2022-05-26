using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LocoMotionUI : MonoBehaviour
{
   [SerializeField] private RectTransform _rectTransform;

   private Tween _tween;

   private void Awake()
   {
      if (_rectTransform == null)
      {
         Debug.LogError(name + " rectTransform is null");
      }
   }

   public void Move(RectTransform moveTo, LocoMotionSO movePram, Action onComplete = null)
   {
      Move(moveTo.anchoredPosition,movePram, onComplete);
   }
   
   public void Move(Vector2 moveTo, LocoMotionSO movePram, Action onComplete = null)
   {
     Move(moveTo,movePram.TimeToTransition,movePram.AnimationCurve,onComplete);
   }
   
   public void Move(Vector2 moveTo,float timeToTransition, AnimationCurve animationCurve = null, Action onComplete = null)
   {
      if (timeToTransition == 0)
      {
         _rectTransform.anchoredPosition = moveTo;
         
         onComplete?.Invoke();
         return;
      }
      _tween = _rectTransform.DOAnchorPos(moveTo, timeToTransition);
      
      if (animationCurve != null)
      {
         _tween.SetEase(animationCurve);
      }

      if (onComplete != null)
      {
         _tween.OnComplete(() => onComplete?.Invoke());
      }
   }

   private void OnDisable()
   {
      _tween.Kill();
   }
}
