using Battle;
using Battle.Turns;
using System;
using UnityEngine;

public class EndTurnButton : ButtonUI
{
    public static event Action OnEndTurnButtonClicked;
    private static EndTurnButton _instance;

    [SerializeField]
    BattleManager _battleManager;
    private GameTurnHandler _turnHandler;
    [SerializeField]
    SoundEventSO OnRejectSound;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _turnHandler = _battleManager.TurnHandler;

        var left = _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
        left.OnTurnActive += ShowTurn;
        left.OnTurnExit += HideTurnButton;
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


}