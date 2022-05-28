using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransitionSO", menuName = "ScriptableObjects/Transitions/Transition")]
public class TransitionsSO : ScriptableObject
{
    [SerializeField] private ScaleSO _scale;
    [SerializeField] private LocoMotionSO _locoMotion;

    public LocoMotionSO LocoMotionSo
    {
        get { return _locoMotion; }
    }

    public ScaleSO ScaleSo
    {
        get { return _scale; }
    }
}
