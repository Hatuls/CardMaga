using CardMaga.UI.Card;
using System;
using CardMaga.Input;
using UnityEngine;

public class BaseHandUIState : MonoBehaviour
{
    public event Action OnEnterState;
    public event Action OnExitState;

    protected InputBehaviour<BattleCardUI> _inputBehaviour;

    protected BattleCardUI _selectedCardUI;


    public BattleCardUI SelectedBattleCardUI
    {
        get => _selectedCardUI;
    }

    private void Awake()
    {
        if (_inputBehaviour == null)
        {
            _inputBehaviour = new InputBehaviour<BattleCardUI>();
        }
    }

    public virtual void EnterState(BattleCardUI battleCardUI)
    {
        if (_selectedCardUI != null)
            return;

        if (!battleCardUI.Inputs.TrySetInputBehaviour(_inputBehaviour))
        {
            Debug.LogError(name + "Failed To Set InputIdentificationSO Behaviour");
            return;
        }

        _selectedCardUI = battleCardUI;

        OnEnterState?.Invoke();
    }

    public virtual void ExitState(BattleCardUI battleCardUI)
    {
        if (!ReferenceEquals(battleCardUI, SelectedBattleCardUI))
        {
            Debug.LogError(name + "CardUI Not equal To the Selected CardUI");
            return;
        }
        
        _selectedCardUI = null;
        OnExitState?.Invoke();
    }

    public virtual BattleCardUI ForceExitState()
    {
        BattleCardUI temp = null;
        
        if (_selectedCardUI != null)
        {
            temp = _selectedCardUI;
            _selectedCardUI.Inputs.ForceResetInputBehaviour();
            _selectedCardUI = null;
        }
        
        OnExitState?.Invoke();
        return temp;
    }

    protected void SetCardUI(CardUI cardUI)
    {
        _selectedCardUI = cardUI;
    }

}
