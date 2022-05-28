using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MotionAbst : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform;

    protected Tween TweenX;
    protected Tween TweenY;
    protected Tween Tween;
    protected Sequence _sequence;

    private void Awake()
    {
        if (rectTransform == null)
        {
            Debug.LogError(name + " rectTransform is null");
        }
    }


    public abstract void Transition(RectTransform rectTransform, TransitionsSO pram, Action onComplete = null);


    public abstract void Transition(Vector2 vector2, TransitionsSO pram, Action onComplete = null);


    public abstract void Transition(Vector2 vector2, float timeToTransition, AnimationCurve animationCurveX = null,
        AnimationCurve animationCurveY = null, Action onComplete = null);
    
    private void OnDisable()
    {
        TweenX.Kill();
        TweenY.Kill();
    }

}
