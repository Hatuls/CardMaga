using CardMaga.Battle;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForFirstCardExecution : MonoBehaviour
{
    private IDisposable _token;
    public void WaitForCardExecution(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        BattleManager.Instance.BattleUIManager.HandUI.OnCardExecutionSuccess += ReleaseToken;
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
