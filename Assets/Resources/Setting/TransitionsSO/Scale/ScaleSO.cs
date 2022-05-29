using UnityEngine;

[CreateAssetMenu(fileName = "ScaleParametersSO", menuName = "ScriptableObjects/Transitions/scale/ScaleParametersSO")]
public class ScaleSO : ScriptableObject , ITransitionable
{
    [Header("Scale Parameters")] 
    
    [SerializeField] private float _timeToTransition = 1.0f;

    [SerializeField] private AnimationCurve _animationCurveX;
    [SerializeField] private AnimationCurve _animationCurveY;

    public AnimationCurve AnimationCurveX
    {
        get { return _animationCurveX; }
    }
    
    public AnimationCurve AnimationCurveY
    {
        get { return _animationCurveY; }
    }

    public float TimeToTransition
    {
        get { return _timeToTransition; }
    }
}
