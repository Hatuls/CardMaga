using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LocoMotionUI : MotionAbst
{
   public override void Transition(RectTransform rectTransform, ITransitionable pram, Action onComplete = null )
   {
      Transition(rectTransform.anchoredPosition,pram, onComplete);
   }
   
   public override void Transition(Vector2 vector2, ITransitionable pram, Action onComplete = null)
   {
     Transition(vector2,pram.TimeToTransition,pram.AnimationCurveX,pram.AnimationCurveY,onComplete);
   }
   
   public override void Transition(Vector2 vector2,float timeToTransition, AnimationCurve animationCurveX = null,AnimationCurve animationCurveY = null, Action onComplete = null)
   {
      if (timeToTransition == 0)
      {
         rectTransform.anchoredPosition = vector2;
         
         onComplete?.Invoke();
         return;
      }
      
      _sequence = DOTween.Sequence();
      
      _sequence.Join(TweenX = rectTransform.DOAnchorPosX(vector2.x, timeToTransition));
      _sequence.Join(TweenY = rectTransform.DOAnchorPosY(vector2.y, timeToTransition));

      if (animationCurveX != null && animationCurveY != null)
      {
         TweenX.SetEase(animationCurveX);
         TweenY.SetEase(animationCurveY);
      }

      if (onComplete != null)
      {
         _sequence.OnComplete(() => onComplete?.Invoke());
      }
   }
}
