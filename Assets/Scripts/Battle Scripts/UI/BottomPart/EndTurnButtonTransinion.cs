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

    [Header("RectTransitionManager")]
    private RectTransitionManager _endTurnTransitionManager;

    void Start()
    {
        _endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        if (_buttonTransitionPackSO != null)
        {
            _endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        }
    }

    [Button]
    public void Scale()
    {
        _endTurnTransitionManager.Scale(_buttonTransitionPackSO);
    }

    [Button]
    public void Transition()
    {
        _endTurnTransitionManager.Transition(_destinationRectTransform, _buttonTransitionPackSO);
    }

}
