using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EndTurnButtonTransinion: MonoBehaviour
{
    [Header("TransitionPackSO")]
    [SerializeField] private TransitionPackSO _buttonTransitionPackSO;

    [Header("RectTransforms")]
    //The objects that will be effected by the animations
    [SerializeField] private RectTransform _currentRectTransform;
    [SerializeField] private RectTransform _destinationRectTransform;
    
    [Button]
    public void Scale()
    {
        _currentRectTransform.Scale(_buttonTransitionPackSO);
    }

    [Button]
    public void Transition()
    {
        _currentRectTransform.Transition(_destinationRectTransform, _buttonTransitionPackSO);
    }

}
