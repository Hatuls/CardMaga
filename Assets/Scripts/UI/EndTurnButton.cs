﻿using Battle.Turns;
using UnityEngine;

public class EndTurnButton : ButtonUI
{

    private static EndTurnButton _instance;
    [SerializeField]
    SoundEventSO OnRejectSound;

    private void Awake()
    {
        _instance = this;
    }

    public override void ButtonPressed()
    {
        if (Battle.Turns.TurnHandler.CurrentState == Battle.Turns.TurnState.PlayerTurn)
        {
            FinishTurn();
        }
        else
        {
            OnRejectSound?.PlaySound();
           // _playSound?.Raise("Reject");
        }
    }


    public static void FinishTurn()
    {
        if (Battle.Turns.TurnHandler.CurrentState == Battle.Turns.TurnState.PlayerTurn && Battle.BattleManager.isGameEnded == false)
        {

            TurnHandler.FinishTurn();
        }
    }
}