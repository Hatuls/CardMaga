using Battle.Turns;
using CardMaga.Battle;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayerSecondTurn : MonoBehaviour
{
    private IDisposable _token;
    public void WaitForPlayerTurn(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        BattleManager.Instance.TurnHandler.GetTurn(GameTurnType.LeftPlayerTurn).OnTurnEnter += CheckTurnCount;
    }

    private void CheckTurnCount()
    {
        if (BattleManager.Instance.TurnHandler.TurnCount != 3)
            return;

        BattleManager.Instance.TurnHandler.GetTurn(GameTurnType.LeftPlayerTurn).OnTurnEnter -= CheckTurnCount;
        ReleaseToken();
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
