using System;
using CardMaga.UI.Card;
using UnityEngine;
using System.Collections.Generic;
using CardMaga.Input;
using CardMaga.UI;
using TMPro;

public class SelectCardUI : BaseHandUIState
{
    [Header("Scripts Reference")]
    [SerializeField] private CardUiInputBehaviourHandler _behaviourHandler;
    [SerializeField] private FollowCardUI _followCardUI;
    [SerializeField] private ZoomCardUI _zoomCardUI;
    [SerializeField] private HandUI _handUI;
    
    // private InputBehaviour<CardUI> _zoomInputBehaviour;
    // private InputBehaviour<CardUI> _followInputBehaviour;
    // private InputBehaviour<CardUI> _handInputBehaviour;
    // private InputBehaviour<CardUI> _selectedInputBehaviour;
    
    private BaseHandUIState _currentState;

    public enum HandState
    {
        Hand,
        Selected,
        Follow,
        Zoom
    };

    private Dictionary<HandState, BaseHandUIState> _handUIStates;
    
    private void SetToZoomState(CardUI cardUI)
    {
       SetState(HandState.Zoom,cardUI);
    }

    private void SetToFollowState(CardUI cardUI)
    {
        SetState(HandState.Follow,cardUI);
    }

    public void SetToSelectedState(CardUI cardUI)
    {
        SetState(HandState.Selected,cardUI);
    }

    private void SetState(HandState state,CardUI cardUI)
    {
        if (_currentState == null)
        {
            _currentState = _handUIStates[state];
        
            _currentState.EnterState(cardUI);
        }
        else
        {
            _currentState.ExitState(cardUI);
            
            _currentState = _handUIStates[state];
        
            _currentState.EnterState(cardUI);
        }
    }

    private void ReturnCardHandState(CardUI cardUI)
    {
        if (_selectedCardUI == null || cardUI != _selectedCardUI)
            return;
        
        SetState(HandState.Hand,cardUI);

        _selectedCardUI = null;
        _currentState = _handUI;
        _currentState.ExitState(cardUI);
    }
    
    // public void ForceReturnCardHandState(CardUI cardUI)
    // {
    //     cardUI.Inputs.ForceSetInputBehaviour(_handInputBehaviour);
    //     _selectedCardUI = null;
    //     _handUI.ForceReturnCardUIToHand(cardUI);
    // }

    private void Start()
    {
        _handUIStates = new Dictionary<HandState, BaseHandUIState>()
        {
            {HandState.Zoom, _zoomCardUI}, {HandState.Follow, _followCardUI}, { HandState.Hand ,_handUI},
 { HandState.Selected ,this}
        };
    }
}
