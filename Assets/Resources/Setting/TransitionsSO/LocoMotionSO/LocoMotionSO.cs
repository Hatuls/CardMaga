using UnityEngine;

[CreateAssetMenu(fileName = "MoveParametersSO", menuName = "ScriptableObjects/Transitions/LocoMotion/MoveParametersSO")]
public class LocoMotionSO : ScriptableObject ,ITransitionable
{
    [Header("Motion Parameters")] 
    
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
