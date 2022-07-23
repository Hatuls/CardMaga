using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonTransinion: MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] TransitionPackSO _endTurnTransitionPackSO;
    
    //Transform of the UI.
    [SerializeField] RectTransform _currentRectTransform;
    [SerializeField] RectTransform _destinationRectTransform;

    //Scrip of thransitions. Recive Transision pack SO
    private RectTransitionManager endTurnTransitionManager;

    void Start()
    {
        endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        if (_endTurnTransitionPackSO != null)
        {
            endTurnTransitionManager = new RectTransitionManager(_currentRectTransform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        endTurnTransitionManager.Transition(_destinationRectTransform, _endTurnTransitionPackSO);
        endTurnTransitionManager.Scale(_endTurnTransitionPackSO);
    }
}
