using ReiTools.TokenMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SectionManager : MonoBehaviour
{
    [SerializeField] OperationManager _operationManager;
    private TokenMachine _tokenMachine;
    private IDisposable _token; 

    public void StartOperations(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _tokenMachine = new TokenMachine(ReleaseToken);
        _operationManager.Init(_tokenMachine);
        _operationManager.StartOperation();
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
