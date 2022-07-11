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

    #region Transitions

    public Sequence Transition(RectTransform destination, TransitionPackSO transitionPackSo,Action onComplete = null)
    {
        return Transition(destination.GetWordPosition(), transitionPackSo,onComplete);
    }
    
    public Sequence Transition(Vector2 destination, TransitionPackSO transitionPackSo,Action onComplete = null)
    {
        Sequence temp = _rectTransform.Move(destination, transitionPackSo.Movement,onComplete);
        
        switch (transitionPackSo.ScaleType)
        {
            case TransitionPackSO.ScaleTypeEnum.ByFloat:
                temp.Join(_rectTransform.Scale(transitionPackSo.ScaleMultiplier, transitionPackSo.Scale,onComplete));
                break;
            case TransitionPackSO.ScaleTypeEnum.ByVector:
                temp.Join(_rectTransform.Scale(transitionPackSo.ScaleVector, transitionPackSo.Scale,onComplete));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (transitionPackSo.HaveRotation)
        {
            temp.Join(_rectTransform.Rotate(transitionPackSo.Rotate, transitionPackSo.Rotation,onComplete));
        }
        
        return temp;
    }


    #endregion

    #region SetPositionAndScale

    public Sequence SetPosition(RectTransform destination, Action onComplete = null)
    {
        return _rectTransform.Move(destination.GetWordPosition(), 0,null,null,null,onComplete);
    }
    
    public Sequence SetPosition(Vector3 destination, Action onComplete = null)
    {
        return _rectTransform.Move(destination, 0,null,null,null,onComplete);
    }
    
    public Tween SetScale(float scale, Action onComplete = null)
    {
        return _rectTransform.Scale(scale, 0,null,onComplete);
    }
    
    public Tween SetScale(Vector3 scale, Action onComplete = null)
    {
        return _rectTransform.Scale(scale,null,onComplete);
    }

    #endregion

    #region Move

    public Sequence Move(RectTransform destination, TransitionPackSO transition,Action onComplete = null)
    {
        return Move(destination.GetWordPosition(), transition, onComplete);
    }
    
    public Sequence Move(Vector2 destination, TransitionPackSO transition,Action onComplete = null)
    {
        return _rectTransform.Move(destination, transition.Movement, onComplete);
    }

    #endregion

    #region Scale
    public Tween Scale(TransitionPackSO transition,Action onComplete = null)
    {
        return _rectTransform.Scale(transition.ScaleMultiplier, transition.Scale, onComplete);
    }
    
    public Tween Scale(float multiply, TransitionPackSO transition,Action onComplete = null)
    {
        return _rectTransform.Scale(multiply, transition.Scale, onComplete);
    }
    
    public Tween Scale(Vector3 scaleVector, TransitionPackSO transition,Action onComplete = null)
    {
        return _rectTransform.Scale(scaleVector, transition.Scale, onComplete);
    }

    #endregion

    #region Rotation

    public Tween Rotate(TransitionPackSO transition,Action onComplete = null)
    {
        return Rotate(transition.Rotate, transition, onComplete);
    }
    public Tween Rotate(Vector3 destination, TransitionPackSO transition,Action onComplete = null)
    {
        return _rectTransform.Rotate(destination, transition.Rotation, onComplete);
    }

    #endregion

    #region TweenManagnent

    private void Kill(ref Tween tween)
    {
        if (tween != null)
        {
            tween.Kill();
        }
    }

    private void Kill(ref Sequence sequence)
    {
        if (sequence != null)
        {
            sequence.Kill();
        }
    }

    #endregion
    
}

#region HelperClass

public static class MoveHelper
{
    public static Sequence Move(this RectTransform rect, RectTransform destination, Transition3D param,
        Action onComplete = null)
    {
        return rect.Move(destination.GetWordPosition(), param, onComplete);
    }

    public static Sequence Move(this RectTransform rect, Vector3 destination, Transition3D param,
        Action onComplete = null)
    {
        return rect.Move(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,param?.AnimationCurveZ,
            onComplete);
    }

    public static Sequence Move(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null,AnimationCurve animationCurveZ = null, Action onComplete = null)
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
        Tween TweenZ = null;

        _sequence.Join(TweenX = rect.DOMoveX(destination.x, timeToTransition));
        _sequence.Join(TweenY = rect.DOMoveY(destination.y, timeToTransition));
        
        if (destination.z != 0)
        {
            _sequence.Join(TweenZ = rect.DOMoveZ(destination.z, timeToTransition));
        }

        
        if (animationCurveX != null) TweenX.SetEase(animationCurveX);

        if (animationCurveY != null) TweenY.SetEase(animationCurveY);
        
        if (TweenZ != null)
        {
            if (animationCurveZ != null) TweenZ.SetEase(animationCurveZ);
        }


        if (onComplete != null) _sequence.OnComplete(() => onComplete?.Invoke());

        return _sequence;
    }
}

public static class RotationHelper
{
    public static Tween Rotate(this RectTransform rect, Vector3 destination, Transition3D param,
        Action onComplete = null)
    {
        return rect.Rotate(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
            param?.AnimationCurveZ,onComplete);;
    }

    public static Tween Rotate(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurveX = null, AnimationCurve animationCurveY = null,
        AnimationCurve animationCurveZ = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.rotation = Quaternion.Euler(destination);

            onComplete?.Invoke();
            return null;
        }
        

        Tween Tween;

        Tween = rect.DORotate(destination, timeToTransition);
        
        if (animationCurveZ != null) Tween.SetEase(animationCurveZ);
        
        if (onComplete != null) Tween.OnComplete(() => onComplete?.Invoke());

        return Tween;
    }
}

public static class ScaleHelper 
{
    public static Tween Scale(this RectTransform rect, float scaleMultiplier, Transition3D param,
        Action onComplete = null)
    {
        return rect.Scale(scaleMultiplier, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
            onComplete);
    }
    
    public static Sequence Scale(this RectTransform rect, Vector3 scaleVector, Transition3D param,
        Action onComplete = null)
    {
        return rect.Scale(scaleVector, param?.TimeToTransition ?? 0, param?.AnimationCurveX,param?.AnimationCurveY,param?.AnimationCurveZ,
            onComplete);
    }
    
    public static Sequence Scale(this RectTransform rect, Vector3 scaleVector, float timeToTransition,
        AnimationCurve animationCurveX = null, AnimationCurve animationCurveY = null,
    AnimationCurve animationCurveZ = null,Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.localScale = scaleVector;

            onComplete?.Invoke();
            return null;
        }

        Sequence _sequence = DOTween.Sequence();
        
        Tween TweenX;
        Tween TweenY;
        Tween TweenZ = null;

        
        _sequence.Join(TweenX = rect.DOScaleX(scaleVector.x, timeToTransition));
        _sequence.Join(TweenY = rect.DOScaleY(scaleVector.y, timeToTransition));

        if (scaleVector.z != 0)
        {
            _sequence.Join(TweenZ = rect.DOScaleZ(scaleVector.z, timeToTransition));
        }

        if (animationCurveX != null) TweenX.SetEase(animationCurveX);

        if (animationCurveY != null) TweenY.SetEase(animationCurveY);

        if (TweenZ != null)
        {
            if (animationCurveZ != null) TweenZ.SetEase(animationCurveZ);
        }
        

        if (onComplete != null) TweenX.OnComplete(() => onComplete?.Invoke());

        return _sequence;
    }
    
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

public static class RectTransformHelper
{
    public static Vector2 GetLocalPosition(this RectTransform rectTransform)
         => rectTransform.transform.localPosition;

    public static Vector2 GetWordPosition(this RectTransform rectTransform)
        => rectTransform.transform.TransformPoint(rectTransform.rect.center);
}

#endregion
