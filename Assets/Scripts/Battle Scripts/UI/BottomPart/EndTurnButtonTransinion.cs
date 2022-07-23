using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EndTurnButtonTransinion: MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] TransitionPackSO _transitionPackSO;
    
    //Transform of the UI.
    [SerializeField] RectTransform _currentRectTransform;
    [SerializeField] RectTransform _destinationRectTransform;

    //Scrip of thransitions. Recive Transision pack SO
    private RectTransitionManager endTurnTransitionManager;

    void Start()
    {
        endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        if (_transitionPackSO != null)
        {
            endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        }
    }

    [Button]
    public void Scale()
    {
        endTurnTransitionManager.Scale(_transitionPackSO);
    }

    [Button]
    public void Transition()
    {
        endTurnTransitionManager.Transition(_destinationRectTransform, _transitionPackSO);

    }

}
