using CardMaga.UI.Card;
using System;
using CardMaga.Input;
using UnityEngine;

public class BaseHandUIState : MonoBehaviour
{
    public event Action OnEnterState;
    public event Action OnExitState;

    protected InputBehaviour<CardUI> _inputBehaviour;

    protected CardUI _selectedCardUI;


    public CardUI SelectedCardUI
    {
        get => _selectedCardUI;
    }

    private void Awake()
    {
        if (_inputBehaviour == null)
        {
            _inputBehaviour = new InputBehaviour<CardUI>();
        }
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

    public virtual CardUI ForceExitState()
    {
        CardUI temp = null;
        
        if (_selectedCardUI != null)
        {
            temp = _selectedCardUI;
            _selectedCardUI.Inputs.ForceResetInputBehaviour();
            _selectedCardUI = null;
        }
        
        OnExitState?.Invoke();
        return temp;
    }



}
