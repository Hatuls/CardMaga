using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using CardMaga.UI.Card;
using UnityEngine;

public class BaseHandUIState : MonoBehaviour
{
    public event Action OnEnterState;
    public event Action OnExitState;
    
    [SerializeField] private CardUIInputBehaviourSO _inputBehaviour;
    
    private CardUI _selectedCardUI;

    public CardUI SelectedCardUI
    {
        get => _selectedCardUI;
    }
    
    protected virtual void EnterState(CardUI cardUI)
    {
        if (_selectedCardUI != null)
            return;

        _selectedCardUI = cardUI;
        
        OnEnterState?.Invoke();
    }

    protected virtual void ExitState(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI, SelectedCardUI))
            return;
        
        _selectedCardUI = null;
        OnExitState?.Invoke();
    }

    protected virtual void ForceExitState(CardUI cardUI)
    {
        _selectedCardUI = null;
        OnExitState?.Invoke();
    }
}
