using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MoveParametersSO", menuName = "ScriptableObjects/LocoMotion/MoveParametersSO")]
public class LocoMotionSO : ScriptableObject
{
    [Header("Motion Parameters")] 
    
    [SerializeField] private float _timeToTransition = 1.0f;

    [SerializeField] private AnimationCurve _animationCurve;

    public AnimationCurve AnimationCurve
    {
        get { return _animationCurve; }
    }

    public float TimeToTransition
    {
        get { return _timeToTransition; }
    }
    
}
