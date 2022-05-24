using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "LocoMotionSO", menuName = "ScriptableObjects/LocoMotion")]
public class LocoMotionSO : ScriptableObject
{
    [Header("Motion Identification")]
    public int Id;

    [Header("Motion Parameters")] 
    [HideInInspector] public int _index;
    public RectTransform[] rectTransforms;

    public float timeToTransition = 1.0f;

    public Ease Ease;
    
}
