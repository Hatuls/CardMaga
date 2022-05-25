using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "LocoMotionSO", menuName = "ScriptableObjects/LocoMotion")]
public class LocoMotionSO : ScriptableObject
{
    [Header("Motion Parameters")] 
    
    public float timeToTransition = 1.0f;

    public Ease Ease;
}
