using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransitionable1D
{
    AnimationCurve AnimationCurveX
    {
        get;
    }
    
    float TimeToTransition
    {
        get;
    }
}

public interface ITransitionable2D : ITransitionable1D
{ 
    
    AnimationCurve AnimationCurveY
    {
        get;
    }
}

public interface ITransitionable3D : ITransitionable2D
{
    AnimationCurve AnimationCurveZ
    {
        get;
    }
}
