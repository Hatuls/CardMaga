using System;
using CardMaga.UI.Card;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SelectCardUI : MonoBehaviour
{
    [Header("Scripts Reference")]
    [SerializeField] private CardUiInputBehaviourHandler _behaviourHandler;
    [SerializeField] private FollowCardUI _followCardUI;
    [SerializeField] private ZoomCardUI _zoomCardUI;
    
    [Header("InputBehaviourSO")]
    [SerializeField] private CardUIInputBehaviourSO _zoomInputBehaviour;
    [SerializeField] private CardUIInputBehaviourSO _followInputBehaviour;
    [SerializeField] private CardUIInputBehaviourSO _handInputBehaviour;
    [SerializeField] private CardUIInputBehaviourSO _selectedInputBehaviour;
    
    private CardUI _selectCardUI;

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

        if (_behaviourHandler.TrySetBehaviour(cardUI.Inputs, _selectedInputBehaviour))
        {
            _selectCardUI = cardUI;
            //let the hand know
        }
    }

    private void SetToZoomState(CardUI cardUI)
    {
        if (!_behaviourHandler.TrySetBehaviour(cardUI.Inputs, _zoomInputBehaviour))
        {
            Debug.LogError("Failed To Set Zoom State");
            return;
        }
        
        _zoomCardUI.SetSelectCardUI(cardUI);
    }

    private void SetToFollowState(CardUI cardUI)
    {
        if (cardUI.Inputs.InputBehaviour == _zoomInputBehaviour)
        {
            if (!_behaviourHandler.TrySetBehaviour(cardUI.Inputs,_followInputBehaviour))
            {
                Debug.LogError("Failed To Set Follow State From Zoom State");
                return;
            }
            
            _zoomCardUI.SetToFollow(cardUI);
        }
        else
        {
            if (!_behaviourHandler.TrySetBehaviour(cardUI.Inputs,_followInputBehaviour))
            {
                Debug.LogError("Failed To Set Follow State");
                return;
            }

            _followCardUI.SetSelectCardUI(cardUI);
        }
    }

    private void SetToHandState(CardUI cardUI)
    {
        if (_selectCardUI == null)
            return;

        if (!_behaviourHandler.TrySetBehaviour(cardUI.Inputs,_handInputBehaviour))
        {
            Debug.LogError("Failed To Set To Hand State");
            return;
        }
        
    }

    private void Start()
    {
        SubEvent();
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
        _followInputBehaviour.OnPointUp += SetToHandState;
        _zoomInputBehaviour.OnClick += SetToHandState;
        _zoomInputBehaviour.OnBeginHold += SetToFollowState;
    }
    
    private void UnSubEvent()
    {
        _handInputBehaviour.OnPointDown -= TrySetSelectedCardUI;
        _selectedInputBehaviour.OnClick -= SetToZoomState;
        _selectedInputBehaviour.OnBeginHold -= SetToFollowState;
        _followInputBehaviour.OnHold -= _followCardUI.FollowHand;
        _followInputBehaviour.OnPointUp -= SetToHandState;
        _zoomInputBehaviour.OnClick -= SetToHandState;
        _zoomInputBehaviour.OnBeginHold -= SetToFollowState;
    }
}
