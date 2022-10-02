using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ReiTools.TokenMachine;

public class BattleTutorial : MonoBehaviour
{
    [SerializeField]
    private OperationManager _operationManager;
    private TokenMachine _tokenMachine;
    public void StartTutorial()
    {
        _tokenMachine = new TokenMachine(TutorialCompleted);
        _operationManager.Init(_tokenMachine);
        _operationManager.StartOperation();
    }
    private void WaitForNextCheckpoint()
    {
        throw new NotImplementedException();
    }
    private void StartCheckpoint()
    {
        throw new NotImplementedException();
    }
    private void ExitTutorial()
    {
        throw new NotImplementedException();
    }

    private void TutorialCompleted()
    {

    }
}
