using System;
using CardMaga.UI.Card;
using UnityEngine;
using System.Collections.Generic;
using CardMaga.UI;
using TMPro;

public class SelectCardUI : MonoBehaviour
{
    [Header("Scripts Reference")]
    [SerializeField] private CardUiInputBehaviourHandler _behaviourHandler;
    [SerializeField] private FollowCardUI _followCardUI;
    [SerializeField] private ZoomCardUI _zoomCardUI;
    [SerializeField] private HandUI _handUI;
    
    [Header("InputBehaviourSO")]
    [SerializeField] private CardUIInputBehaviourSO _zoomInputBehaviour;
    [SerializeField] private CardUIInputBehaviourSO _followInputBehaviour;
    [SerializeField] private CardUIInputBehaviourSO _handInputBehaviour;
    [SerializeField] private CardUIInputBehaviourSO _selectedInputBehaviour;
    
    private CardUI _selectCardUI;

    private BaseHandUIState _currentState;

    public enum HandState
    {
        Hand,
        Selected,
        Follow,
        Zoom
    };

    private Dictionary<HandState, BaseHandUIState> _handUIStates;

    public CardUI SelectedCardUI
    {
        get => _selectCardUI;
    }

    public void TrySetSelectedCardUI(CardUI cardUI)
    {
        if (_selectCardUI != null)
        {
            Debug.LogWarning("Selected Card already have a value");
            return;
        }

        if (cardUI.Inputs.TrySetInputBehaviour(_selectedInputBehaviour))
        {
            _selectCardUI = cardUI;
            //let the hand know
        }
    }

    private void SetToZoomState(CardUI cardUI)
    {
       SetState(HandState.Zoom,cardUI);
    }

    private void SetToFollowState(CardUI cardUI)
    {
        SetState(HandState.Follow,cardUI);
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
        if (_selectCardUI == null || cardUI != _selectCardUI)
            return;

        if (!cardUI.Inputs.TrySetInputBehaviour(_handInputBehaviour))
        {
            Debug.LogError("Failed To Set To Hand State");
            return;
        }

        _selectCardUI = null;
        _currentState = null;
        _handUI.ReturnCardUIToHand(cardUI);
    }
    
    private void ForceReturnCardHandState(CardUI cardUI)
    {
        cardUI.Inputs.ForceSetInputBehaviour(_handInputBehaviour);
        _selectCardUI = null;
        _handUI.ForceReturnCardUIToHand(cardUI);
    }

    private void Start()
    {
        SubEvent();

        _handUIStates = new Dictionary<HandState, BaseHandUIState>()
        {
            {HandState.Zoom, _zoomCardUI}, {HandState.Follow, _followCardUI}
        };
    }

    private void OnDestroy()
    {
        UnSubEvent();
    }

    private void SubEvent()
    {
        _handInputBehaviour.OnPointDown += TrySetSelectedCardUI;
        _selectedInputBehaviour.OnClick += SetToZoomState;
        _selectedInputBehaviour.OnBeginHold += SetToFollowState;
        _followInputBehaviour.OnHold += _followCardUI.FollowHand;
        _followInputBehaviour.OnPointUp += ReturnCardHandState;
        _zoomInputBehaviour.OnClick += ReturnCardHandState;
        _zoomInputBehaviour.OnBeginHold += SetToFollowState;
    }
    
    private void UnSubEvent()
    {
        _handInputBehaviour.OnPointDown -= TrySetSelectedCardUI;
        _selectedInputBehaviour.OnClick -= SetToZoomState;
        _selectedInputBehaviour.OnBeginHold -= SetToFollowState;
        _followInputBehaviour.OnHold -= _followCardUI.FollowHand;
        _followInputBehaviour.OnPointUp -= ReturnCardHandState;
        _zoomInputBehaviour.OnClick -= ReturnCardHandState;
        _zoomInputBehaviour.OnBeginHold -= SetToFollowState;
    }
}
