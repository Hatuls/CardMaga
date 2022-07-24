using System;
using DG.Tweening;
using UnityEngine;

public static class RectTransitionManager
{
    #region PublicFunction
    
    #region Transitions
    
    public static Sequence Transition(this RectTransform rectTransform ,TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        Vector3 destination = (Vector3)rectTransform.GetWordPosition() + transitionPackSo.MoveOffSet;
        return rectTransform.Transition(destination, transitionPackSo, onComplete);
    }
    
    public static Sequence Transition(this RectTransform rectTransform , RectTransform destination, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Transition(destination.GetWordPosition(), transitionPackSo, onComplete);
    }

    public static Sequence Transition(this RectTransform rectTransform ,Vector2 destination, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();

        if (transitionPackSo.HaveMovement)
            sequence.Join(rectTransform.DoMove(destination, transitionPackSo));

        if (transitionPackSo.HaveScale)
        {
            switch (transitionPackSo.ScaleType)
            {
                case TransitionPackSO.ScaleTypeEnum.ByFloat:
                    sequence.Join(rectTransform.Scale(transitionPackSo.ScaleMultiplier, transitionPackSo.Scale));
                    break;
                case TransitionPackSO.ScaleTypeEnum.ByVector:
                    sequence.Join(rectTransform.Scale(transitionPackSo.ScaleVector, transitionPackSo.Scale));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (transitionPackSo.HaveRotation)
            sequence.Join(rectTransform.Rotate(transitionPackSo.Rotate, transitionPackSo.Rotation));

        if (onComplete != null)
            sequence.OnComplete(onComplete.Invoke);

        return sequence;
    }

    #endregion

    #region SetPositionAndScale

    public static Sequence SetPosition(this RectTransform rectTransform ,RectTransform destination, Action onComplete = null)
    {
        return rectTransform.SetPosition(destination.GetWordPosition(),onComplete);
    }

    public static Sequence SetPosition(this RectTransform rectTransform ,Vector3 destination, Action onComplete = null)
    {
        return rectTransform.DoMove(destination,null,onComplete);
    }
    
    public static Tween SetScale(this RectTransform rectTransform ,TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Scale(transitionPackSo, onComplete);
    }
    
    public static Tween SetScale(this RectTransform rectTransform ,float scale, Action onComplete = null)
    {
        return rectTransform.Scale(scale, 0, null, onComplete);
    }

    #endregion

    #region Move
    
    public static Sequence Move(this RectTransform rectTransform, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        Vector3 destination = (Vector3)rectTransform.GetWordPosition() + transitionPackSo.MoveOffSet;
        return rectTransform.Move(destination, transitionPackSo, onComplete);
    }
    
    public static Sequence Move(this RectTransform rectTransform,RectTransform destination, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Move(destination.GetWordPosition(), transitionPackSo, onComplete);
    }

    public static Sequence Move(this RectTransform rectTransform,Vector2 destination, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.DoMove(destination, transitionPackSo, onComplete);
    }

    #endregion

    #region Scale

    public static Tween Scale(this RectTransform rectTransform,TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        switch (transitionPackSo.ScaleType)
        {
            case TransitionPackSO.ScaleTypeEnum.ByFloat:
                return rectTransform.Scale(transitionPackSo.ScaleMultiplier, transitionPackSo.Scale, onComplete);
            case TransitionPackSO.ScaleTypeEnum.ByVector:
                return rectTransform.Scale(transitionPackSo.ScaleVector, transitionPackSo.Scale, onComplete);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static Tween Scale(this RectTransform rectTransform,float multiply, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Scale(multiply, transitionPackSo.Scale, onComplete);
    }

    public static Tween Scale(this RectTransform rectTransform,Vector3 scaleVector, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Scale(scaleVector, transitionPackSo.Scale, onComplete);
    }

    #endregion

    #region Rotation

    public static Tween Rotate(this RectTransform rectTransform,TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Rotate(transitionPackSo.Rotate, transitionPackSo, onComplete);
    }

    public static Tween Rotate(this RectTransform rectTransform,Vector3 destination, TransitionPackSO transitionPackSo, Action onComplete = null)
    {
        return rectTransform.Rotate(destination, transitionPackSo.Rotation, onComplete);
    }

    #endregion

    #endregion

    #region PrivateFunction

    #region Move
    
    private static Sequence DoMove(this RectTransform rect, Vector3 destination, TransitionPackSO transitionPackSo = null,
        Action onComplete = null)
    {
        Transition3D param = null;
        
        if (transitionPackSo != null)
        {
            param = transitionPackSo.Movement;
        }

        switch (transitionPackSo.MovePositionType)
        {
            case TransitionPackSO.PositionType.AnchoredPosition:
                return rect.MoveAnchorPosition(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
                    onComplete);
            case TransitionPackSO.PositionType.WordPosition:
                return rect.MoveWordPosition(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
                    param?.AnimationCurveZ,
                    onComplete);
            case TransitionPackSO.PositionType.LocalPosition:
                return rect.MoveLocalPosition(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
                    param?.AnimationCurveZ,
                    onComplete);
            default:
                return rect.MoveAnchorPosition(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
                    onComplete);
        }
    }

    private static Sequence MoveWordPosition(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null, AnimationCurve animationCurveZ = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.position = destination;

            onComplete?.Invoke();
            return null;
        }

        var sequence = DOTween.Sequence();

        Tween tweenX;
        Tween tweenY;
        Tween tweenZ = null;

        sequence.Join(tweenX = rect.DOMoveX(destination.x, timeToTransition));
        sequence.Join(tweenY = rect.DOMoveY(destination.y, timeToTransition));

        if (destination.z != 0) sequence.Join(tweenZ = rect.DOMoveZ(destination.z, timeToTransition));


        if (animationCurveX != null) tweenX.SetEase(animationCurveX);

        if (animationCurveY != null) tweenY.SetEase(animationCurveY);

        if (tweenZ != null)
            if (animationCurveZ != null)
                tweenZ.SetEase(animationCurveZ);


        if (onComplete != null) sequence.OnComplete(() => onComplete?.Invoke());

        return sequence;
    }
    
    private static Sequence MoveLocalPosition(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null, AnimationCurve animationCurveZ = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.localPosition = destination;

            onComplete?.Invoke();
            return null;
        }

        var sequence = DOTween.Sequence();

        Tween tweenX;
        Tween tweenY;
        Tween tweenZ = null;

        sequence.Join(tweenX = rect.DOLocalMoveX(destination.x, timeToTransition));
        sequence.Join(tweenY = rect.DOLocalMoveY(destination.y, timeToTransition));

        if (destination.z != 0) sequence.Join(tweenZ = rect.DOLocalMoveZ(destination.z, timeToTransition));


        if (animationCurveX != null) tweenX.SetEase(animationCurveX);

        if (animationCurveY != null) tweenY.SetEase(animationCurveY);

        if (tweenZ != null)
            if (animationCurveZ != null)
                tweenZ.SetEase(animationCurveZ);


        if (onComplete != null) sequence.OnComplete(() => onComplete?.Invoke());

        return sequence;
    }
    
    private static Sequence MoveAnchorPosition(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.anchoredPosition = destination;

            onComplete?.Invoke();
            return null;
        }

        var sequence = DOTween.Sequence();

        Tween tweenX;
        Tween tweenY;
        Tween tweenZ = null;

        sequence.Join(tweenX = rect.DOAnchorPosX(destination.x, timeToTransition));
        sequence.Join(tweenY = rect.DOAnchorPosY(destination.y, timeToTransition));
        
        if (animationCurveX != null) tweenX.SetEase(animationCurveX);

        if (animationCurveY != null) tweenY.SetEase(animationCurveY);
        
        if (onComplete != null) sequence.OnComplete(() => onComplete?.Invoke());

        return sequence;
    }
    
    #endregion

    #region Scale

    private static Tween Scale(this RectTransform rect, float scaleMultiplier, Transition3D param = null,
        Action onComplete = null)
    {
        return rect.Scale(scaleMultiplier, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
            onComplete);
    }

    private static Sequence Scale(this RectTransform rect, Vector3 scaleVector, Transition3D param,
        Action onComplete = null)
    {
        return rect.Scale(scaleVector, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
            param?.AnimationCurveZ,
            onComplete);
    }

    private static Sequence Scale(this RectTransform rect, Vector3 scaleVector, float timeToTransition,
        AnimationCurve animationCurveX = null, AnimationCurve animationCurveY = null,
        AnimationCurve animationCurveZ = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.localScale = scaleVector;

            onComplete?.Invoke();
            return null;
        }

        var sequence = DOTween.Sequence();

        Tween tweenX;
        Tween tweenY;
        Tween tweenZ = null;


        sequence.Join(tweenX = rect.DOScaleX(scaleVector.x, timeToTransition));
        sequence.Join(tweenY = rect.DOScaleY(scaleVector.y, timeToTransition));

        if (scaleVector.z != 0) sequence.Join(tweenZ = rect.DOScaleZ(scaleVector.z, timeToTransition));

        if (animationCurveX != null) tweenX.SetEase(animationCurveX);

        if (animationCurveY != null) tweenY.SetEase(animationCurveY);

        if (tweenZ != null)
            if (animationCurveZ != null)
                tweenZ.SetEase(animationCurveZ);


        if (onComplete != null) tweenX.OnComplete(() => onComplete?.Invoke());

        return sequence;
    }

    private static Tween Scale(this RectTransform rect, float scaleMultiplier, float timeToTransition,
        AnimationCurve animationCurveX = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.localScale *= scaleMultiplier;

            onComplete?.Invoke();
            return null;
        }

        Tween tweenX = rect.DOScale(Vector3.one * scaleMultiplier, timeToTransition);

        if (animationCurveX != null) tweenX.SetEase(animationCurveX);


        if (onComplete != null) tweenX.OnComplete(() => onComplete?.Invoke());

        return tweenX;
    }

    #endregion

    #region Rotate

    private static Tween Rotate(this RectTransform rect, Vector3 destination, Transition3D param,
        Action onComplete = null)
    {
        return rect.Rotate(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX, param?.AnimationCurveY,
            param?.AnimationCurveZ, onComplete);
        ;
    }

    private static Tween Rotate(this RectTransform rect, Vector3 destination, float timeToTransition,
        AnimationCurve animationCurveX = null, AnimationCurve animationCurveY = null,
        AnimationCurve animationCurveZ = null, Action onComplete = null)
    {
        if (timeToTransition == 0)
        {
            rect.rotation = Quaternion.Euler(destination);

            onComplete?.Invoke();
            return null;
        }


        Tween tween = rect.DORotate(destination, timeToTransition);

        if (animationCurveZ != null) tween.SetEase(animationCurveZ);

        if (onComplete != null) tween.OnComplete(() => onComplete?.Invoke());

        return tween;
    }

    #endregion

    #endregion   

    #region TweenManagnent

    private static void Kill(ref Tween tween)
    {
        if (tween != null) tween.Kill();
    }

    private static void Kill(ref Sequence sequence)
    {
        if (sequence != null) sequence.Kill();
    }

    #endregion
}

#region HelperClass

public static class RectTransformHelper
{
    public static Vector2 GetLocalPosition(this RectTransform rectTransform)
    {
        return rectTransform.transform.localPosition;
    }

    public static Vector2 GetWordPosition(this RectTransform rectTransform)
    {
        return rectTransform.transform.TransformPoint(rectTransform.rect.center);
    }
}

#endregion