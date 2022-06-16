using System;
using UnityEngine;

[Serializable]
public class Transition3DData :Transition2DData, ITransitionable3D
{
    [SerializeField] private AnimationCurve _animationCurveZ;
    public AnimationCurve AnimationCurveZ
    {
        get { return _animationCurveZ; }
    }

   
}

[Serializable]
public class Transition2DData :Transition1DData, ITransitionable2D
{
  [SerializeField] private AnimationCurve _animationCurveY;
    public AnimationCurve AnimationCurveY
    {
        get { return _animationCurveY; }
    }

   
}
[Serializable]
public class Transition1DData :ITransitionable1D
{
    [SerializeField] private float _timeToTransition = 1.0f;

    [SerializeField] private AnimationCurve _animationCurveX;
    public float TimeToTransition
    {
        get { return _timeToTransition; }
    }
    public AnimationCurve AnimationCurveX
    {
        get { return _animationCurveX; }
    }
}