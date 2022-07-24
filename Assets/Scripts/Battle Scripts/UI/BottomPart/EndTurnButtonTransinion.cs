using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EndTurnButtonTransinion: MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private TransitionPackSO _transitionPackSO;
    
    //Transform of the UI.
    [SerializeField] private RectTransform _currentRectTransform;
    [SerializeField] private RectTransform _destinationRectTransform;

    [Button]
    public void Scale()
    {
        _currentRectTransform.Scale(_transitionPackSO);
    }

    [Button]
    public void Transition()
    {
        _currentRectTransform.Transition(_destinationRectTransform, _transitionPackSO);
    }

}
