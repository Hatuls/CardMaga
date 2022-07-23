using System;
using UnityEngine;

[Serializable]
public class Transition3D :Transition2D
{
    [SerializeField] private AnimationCurve _animationCurveZ;
    public AnimationCurve AnimationCurveZ
    {
        get { return _animationCurveZ;}
#if UNITY_EDITOR
        set { _animationCurveZ = value; }

#endif
    }
} 

[Serializable]
public class Transition2D :Transition1D
{
    [SerializeField] private AnimationCurve _animationCurveY;
    public AnimationCurve AnimationCurveY
    {
        get { return _animationCurveY;}
#if UNITY_EDITOR
        set { _animationCurveY = value; }

#endif
    }

   
}
[Serializable]
public class Transition1D
{
    [SerializeField] private float _timeToTransition = 1.0f;

    [SerializeField] private AnimationCurve _animationCurveX;
    public float TimeToTransition
    {
        get { return _timeToTransition;}
#if UNITY_EDITOR
        set { _timeToTransition = value; }
        
#endif
    }
    public AnimationCurve AnimationCurveX
    {
        get { return _animationCurveX;}
#if UNITY_EDITOR
        set { _animationCurveX = value; }

#endif
    }
}