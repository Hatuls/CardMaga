using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveParametersSO", menuName = "ScriptableObjects/LocoMotion/MoveParametersSO")]
public class LocoMotionSO : ScriptableObject
{
    [Header("Motion Parameters")] 
    
    public float timeToTransition = 1.0f;

    public List<RectTransform> Motions;
    
    public Ease Ease;
    
    
}
