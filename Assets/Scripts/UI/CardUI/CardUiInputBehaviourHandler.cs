using System;
using System.Collections.Generic;
using CardMaga.Input;
using CardMaga.UI.Card;
using UnityEngine;

public class CardUiInputBehaviourHandler : InputBehaviourHandler<BattleCardUI>
{
    [SerializeField] private BaseHandUIState _handUIState;
    [SerializeField] private BaseHandUIState _zoomUIState;
    [SerializeField] private BaseHandUIState _followUIState;
    
    [SerializeField] private BaseHandUIState _defualtState;
    private BaseHandUIState _currentState;
    
    public enum HandState
    {
        Hand,
        Follow,
        Zoom
    };

    private Dictionary<HandState, BaseHandUIState> _handUIStates;

    private void Awake()
    {
        _handUIStates = new Dictionary<HandState, BaseHandUIState>()
        {
            {HandState.Zoom, _zoomUIState}, {HandState.Follow, _followUIState}, { HandState.Hand ,_handUIState},
        };
    }

    public void SetState(HandState state,BattleCardUI battleCardUI)
    {
        if (_currentState == null)
        {
            _currentState = _handUIStates[state];
        
            _currentState.EnterState(battleCardUI);
        }
        else
        {
            _currentState.ExitState(battleCardUI);
            
            _currentState = _handUIStates[state];
        
            _currentState.EnterState(battleCardUI);
        }
    }
}
