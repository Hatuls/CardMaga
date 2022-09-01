using Battle;
using Battle.Turns;
using Managers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class EndTurnButton : ButtonUI, ISequenceOperation<BattleManager>
{
    public static event Action OnEndTurnButtonClicked;

    [SerializeField]
    private GameObject _visualizer;
    [SerializeField]
    SoundEventSO OnRejectSound;

    public int Priority => 0;
    private bool _isDirty;
    private void Awake()
    {

        BattleManager.Register(this, OrderType.Before);
    }


    private void ShowTurn() { _visualizer.SetActive(true); _isDirty = false; }
    private void HideTurnButton() => _visualizer.SetActive(false);

    public override void ButtonPressed()
    {
        if (!_isDirty)
        {
            _isDirty = true;
            OnEndTurnButtonClicked?.Invoke();
        }

        //   OnRejectSound?.PlaySound();

    }

    public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
    {

        OnEndTurnButtonClicked += data.TurnHandler.MoveToNextTurn;
        var left = data.TurnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive += ShowTurn;
        left.OnTurnExit += HideTurnButton;
        data.OnBattleManagerDestroyed += BeforeDestroyed;
        HideTurnButton();
    }

    private void BeforeDestroyed(BattleManager bm)
    {
        var _turnHandler = bm.TurnHandler;
        OnEndTurnButtonClicked -= _turnHandler.MoveToNextTurn;
        bm.OnBattleManagerDestroyed -= BeforeDestroyed;
        var left = _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive -= ShowTurn;
        left.OnTurnExit -= HideTurnButton;
        _turnHandler = null;
    }
}