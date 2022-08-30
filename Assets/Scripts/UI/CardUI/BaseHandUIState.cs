﻿using CardMaga.UI.Card;
using System;
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

    public virtual void EnterState(CardUI cardUI)
    {
        if (_selectedCardUI != null)
            return;

        if (!cardUI.Inputs.TrySetInputBehaviour(_inputBehaviour))
        {
            Debug.LogError(name + "Failed To Set Input Behaviour");
            return;
        }

        _selectedCardUI = cardUI;

        OnEnterState?.Invoke();
    }

    public virtual void ExitState(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI, SelectedCardUI))
        {
            Debug.LogError(name + "CardUI Not equal To the Selected CardUI");
            return;
        }

        _selectedCardUI = null;
        OnExitState?.Invoke();
    }

    public virtual void ForceExitState(CardUI cardUI)
    {
        _selectedCardUI = null;
        OnExitState?.Invoke();
    }



}
