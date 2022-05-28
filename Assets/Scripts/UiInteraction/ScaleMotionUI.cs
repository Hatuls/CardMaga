using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleMotionUI : MotionAbst
{
    public override void Transition(RectTransform rectTransform, TransitionsSO pram, Action onComplete = null)
    {
        Transition(rectTransform.anchoredPosition,pram,onComplete);
    }

    public override void Transition(Vector2 vector2, TransitionsSO pram, Action onComplete = null)
    {
        Transition(vector2,pram.ScaleSo.TimeToTransition,pram.ScaleSo.AnimationCurveX,pram.ScaleSo.AnimationCurveY,onComplete);
    }

    public override void Transition(Vector2 vector2, float timeToTransition, AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rectTransform.localScale = vector2;
         
            onComplete?.Invoke();
            return;
        }
        
        _sequence = DOTween.Sequence();

        _sequence.Join(TweenX = rectTransform.DOScaleX(vector2.x, timeToTransition));
        _sequence.Join(TweenY = rectTransform.DOScaleY(vector2.y, timeToTransition));
        
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
