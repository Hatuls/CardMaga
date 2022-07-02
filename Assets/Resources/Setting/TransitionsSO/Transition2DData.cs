using System;
using UnityEngine;

[Serializable]
public class Transition3D :Transition2D
{
    [SerializeField] private AnimationCurve _animationCurveZ;
    public AnimationCurve AnimationCurveZ
    {
        get;
        set;
    }
} 

[Serializable]
public class Transition2D :Transition1D
{
  [SerializeField] private AnimationCurve _animationCurveY;
    public AnimationCurve AnimationCurveY
    {
        get;
        set;
    }

   
}
[Serializable]
public class Transition1D
{
    [SerializeField] private float _timeToTransition = 1.0f;

    [SerializeField] private AnimationCurve _animationCurveX;
    public float TimeToTransition
    {
        get;
        set;    
    }
    public AnimationCurve AnimationCurveX
    {
        get;
        set;
    }
}