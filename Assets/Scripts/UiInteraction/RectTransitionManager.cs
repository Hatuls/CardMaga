using System;
using DG.Tweening;
using UnityEngine;


public class RectTransitionManager
{
    private RectTransform _rectTransform;

    public RectTransitionManager(RectTransform rectTransform)
    {
        _rectTransform = rectTransform;
    }
    
    public void Transition(RectTransform destination, TransitionPackSO transitionPackSo)
    {
        _rectTransform.Move(destination, transitionPackSo.Movement);
        _rectTransform.Scale(1f,transitionPackSo.Scale);//test need work
        _rectTransform.Rotate(destination, transitionPackSo.Rotation);
    }
    
    public void Transition(Vector2 destination, TransitionPackSO transitionPackSo)
    {
        _rectTransform.Move(destination, transitionPackSo.Movement);
        _rectTransform.Scale(1f,transitionPackSo.Scale);//test need work
        //_rectTransform.Rotate(destination, transitionPackSo.Rotation);
    }

    public void SetPosition(RectTransform destination, Action onComplete = null)
    {
        Vector2 worldPos =  destination.transform.TransformPoint(destination.rect.center);
        _rectTransform.Move(worldPos, 0,null,null,onComplete);
    }
    
    public void SetScale(float scale, Action onComplete = null)
    {
        _rectTransform.Scale(scale, 0,null,onComplete);
    }
    
    public void Move(RectTransform destination, ITransitionReciever transition,Action onComplete = null)
    {
        _rectTransform.Move(destination, transition.Movement, onComplete);
    }
    
    public void Move(Vector2 destination, ITransitionReciever transition,Action onComplete = null)
    {
        _rectTransform.Move(destination, transition.Movement, onComplete);
    }
    
    public void Scale(float multiply, ITransitionReciever transition,Action onComplete = null)
    {
        _rectTransform.Scale(multiply, transition.Scale, onComplete);
    }
    public void Rotate(RectTransform destination, ITransitionReciever transition,Action onComplete = null)
    {
        _rectTransform.Rotate(destination, transition.Rotation, onComplete);
    }
    public void Rotate(Vector3 destination, ITransitionReciever transition,Action onComplete = null)
    {
        _rectTransform.Rotate(destination, transition.Rotation, onComplete);
    }
}


public static class MoveHelper
{
    public static Sequence Move(this RectTransform rect, RectTransform destination, ITransitionable2D param,
        Action onComplete = null)
    {
        Vector2 worldPos = rect.transform.TransformPoint(destination.rect.center);
        return rect.Move(worldPos, param, onComplete);
    }

    public static Sequence Move(this RectTransform rect, Vector2 destination, ITransitionable2D param,
        Action onComplete = null)
    {
        return rect.Move(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
            onComplete);
    }

    public static Sequence Move(this RectTransform rect, Vector2 destination, float timeToTransition,
        AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.position = destination;

            onComplete?.Invoke();
            return null;
        }

        var _sequence = DOTween.Sequence();

        Tween TweenX;
        Tween TweenY;

        _sequence.Join(TweenX = rect.DOMoveX(destination.x, timeToTransition));
        _sequence.Join(TweenY = rect.DOMoveY(destination.y, timeToTransition));

        if (animationCurveX != null) TweenX.SetEase(animationCurveX);

        if (animationCurveY != null) TweenY.SetEase(animationCurveY);


        if (onComplete != null) _sequence.OnComplete(() => onComplete?.Invoke());

        return _sequence;
    }
}

public static class RotationHelper
{
    public static Tween Rotate(this RectTransform rect, RectTransform destination, ITransitionable1D param,
        Action onComplete = null)
    {
        var destinationRotation = destination.eulerAngles;
        return rect.Rotate(destinationRotation, param, onComplete);
    }

    public static Tween Rotate(this RectTransform rect, Vector2 destination, ITransitionable1D param,
        Action onComplete = null)
    {
        return rect.Rotate(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
            onComplete);
    }

    public static Tween Rotate(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurve = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.rotation = Quaternion.Euler(destination);

            onComplete?.Invoke();
            return null;
        }
        

        Tween Tween;

        Tween = rect.DORotate(destination, timeToTransition);
        
        if (animationCurve != null) Tween.SetEase(animationCurve);
        

        if (onComplete != null) Tween.OnComplete(() => onComplete?.Invoke());

        return Tween;
    }
}

public static class ScaleHelper 
{
    public static Tween Scale(this RectTransform rect, float scaleMultiplier, ITransitionable1D param,
        Action onComplete = null)
    {
        return rect.Scale(scaleMultiplier, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
            onComplete);
    }
    
// Fix it WIP
    public static Tween Scale(this RectTransform rect, float scaleMultiplier, float timeToTransition,
        AnimationCurve animationCurveX = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.localScale *= scaleMultiplier;

            onComplete?.Invoke();
            return null;
        }
        
        Tween TweenX;
        
        TweenX = rect.DOScale(Vector3.one * scaleMultiplier, timeToTransition);
        
        if (animationCurveX != null) TweenX.SetEase(animationCurveX);


        if (onComplete != null) TweenX.OnComplete(() => onComplete?.Invoke());

        return TweenX;
    }
}