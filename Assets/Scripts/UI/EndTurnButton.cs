using Battle;
using Battle.Turns;
using Managers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class EndTurnButton : ButtonUI , ISequenceOperation<BattleManager>
{
    public static event Action OnEndTurnButtonClicked;


    private GameTurnHandler _turnHandler;
    [SerializeField]
    SoundEventSO OnRejectSound;

    public int Priority => 0;

    private void Awake()
    {

        BattleManager.Register(this, OrderType.Before);
    }

    private void OnDestroy()
    {

        var left = _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive -= ShowTurn;
        left.OnTurnExit   -= HideTurnButton;
        _turnHandler      = null;
    }

    private void ShowTurn() =>  gameObject.SetActive(true);
    private void HideTurnButton() => gameObject.SetActive(false);

    public override void ButtonPressed()
    {
        if (_turnHandler.CurrentTurn ==  GameTurnType.LeftPlayerTurn)
        {
            OnEndTurnButtonClicked?.Invoke();
        }
        else
        {
            OnRejectSound?.PlaySound();
           // _playSound?.Raise("Reject");
        }
    }

    public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
    {
        _turnHandler = data.TurnHandler;
        var left = _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive += ShowTurn;
        left.OnTurnExit += HideTurnButton;
        HideTurnButton();
    }
}