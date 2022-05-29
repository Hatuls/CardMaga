using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransitionable 
{ 
    AnimationCurve AnimationCurveX
    {
        get;
    }
    AnimationCurve AnimationCurveY
    {
        get;
    }

     float TimeToTransition
    {
        get;
    }
}
