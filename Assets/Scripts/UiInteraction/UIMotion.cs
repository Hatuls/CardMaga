using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class UIMotion
{
    [Header("Motion Identification")]
    public String name;
    
    public int Id;
    
    [Header("Motion Parameters")]
    public Vector2 _dis;

    public float timeToTransition = 1.0f;

    public Ease Ease;
    
}
