using Battle;
using Battle.Turns;
using CardMaga.Sequence;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class EndTurnButton : ButtonUI, ISequenceOperation<IBattleManager>
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


    private void ShowTurn()
    {
        _visualizer.SetActive(true);
        _isDirty = false;
    }
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

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {

        OnEndTurnButtonClicked += data.PlayersManager.GetCharacter(true).EndTurnHandler.EndTurnPressed;
        GameTurn left = data.TurnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive += ShowTurn;
        left.OnTurnExit += HideTurnButton;
        data.OnBattleManagerDestroyed += BeforeDestroyed;
        BattleManager.OnGameEnded += HideTurnButton;
        HideTurnButton();
    }

    private void BeforeDestroyed(IBattleManager bm)
    {
        BattleManager.OnGameEnded -= HideTurnButton;
        var _turnHandler = bm.TurnHandler;
        OnEndTurnButtonClicked -= bm.PlayersManager.GetCharacter(true).EndTurnHandler.EndTurnPressed;
        bm.OnBattleManagerDestroyed -= BeforeDestroyed;
        var left = _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive -= ShowTurn;
        left.OnTurnExit   -= HideTurnButton;
        _turnHandler = null;
    }
}