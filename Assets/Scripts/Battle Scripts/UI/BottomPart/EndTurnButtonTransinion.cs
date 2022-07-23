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

    //Scrip of thransitions. Recive Transision pack SO
    private RectTransitionManager _endTurnTransitionManager;

    void Start()
    {
        _endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        if (_transitionPackSO != null)
        {
            _endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        }
    }

    [Button]
    public void Scale()
    {
        _endTurnTransitionManager.Scale(_transitionPackSO);
    }

    [Button]
    public void Transition()
    {
        _endTurnTransitionManager.Transition(_destinationRectTransform, _transitionPackSO);
    }

}
