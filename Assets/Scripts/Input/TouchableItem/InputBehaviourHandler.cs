using CardMaga.Input;
using CardMaga.UI.Card;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputBehaviourHandler<T> : MonoBehaviour where T : MonoBehaviour
{
    private BaseHandUIState _currentState;
    private InputBehaviourState _currentStateID;

    protected Dictionary<InputBehaviourState, BaseHandUIState> _handUIStates;

    public InputBehaviourState CurrentStateID
    {
        get => _currentStateID;
    }

    protected void SetState(InputBehaviourState state, BattleCardUI battleCardUI)
    {
        if (_currentState != null && state != battleCardUI.Inputs.CurrentInputBehaviourState)
            _currentState.ExitState(battleCardUI);

        _currentState = _handUIStates[state];

        _currentStateID = state;

        battleCardUI.Inputs.ChangeState(_currentStateID);


        if (_currentState == null)
        {
            battleCardUI.Inputs.ForceResetInputBehaviour();
            return;
        }


        _currentState.EnterState(battleCardUI);
    }

    protected void SetAllTouchableItemsToDefault(TouchableItem<T>[] touchableItems)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;

            touchableItems[i].ForceResetInputBehaviour();
        }
    }
}

public enum InputBehaviourState
{
    Default,
    Collection,
    Hand,
    HandFollow,
    HandZoom,
};

