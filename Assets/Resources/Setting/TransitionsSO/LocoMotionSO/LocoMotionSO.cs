using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MoveParametersSO", menuName = "ScriptableObjects/Transitions/LocoMotion/MoveParametersSO")]
public class LocoMotionSO : ScriptableObject
{
    [Header("Motion Parameters")] 
    
    [SerializeField] private float _timeToTransition = 1.0f;

    [SerializeField] private AnimationCurve _animationCurveX;
    [SerializeField] private AnimationCurve _animationCurveY;

    public AnimationCurve AnimationCurveX
    {
        get { return _animationCurveX; }
    }
    
    public AnimationCurve AnimationCurveY
    {
        get { return _animationCurveY; }
    }

    public float TimeToTransition
    {
        get { return _timeToTransition; }
    }
    
}
