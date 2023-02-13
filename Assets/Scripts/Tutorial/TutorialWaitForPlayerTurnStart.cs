using Battle.Turns;
using CardMaga.Battle;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaitForPlayerTurnStart : MonoBehaviour
{
    private IDisposable _token;
    public void WaitForPlayerFirstTurn(ITokenReceiver tokenReciever)
    {
        _token = tokenReciever.GetToken();
        BattleManager.Instance.TurnHandler.GetTurn(GameTurnType.EnterBattle).OnTurnEnter += ReleaseToken;
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
