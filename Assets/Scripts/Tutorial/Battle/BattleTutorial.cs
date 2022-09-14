using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleTutorial : MonoBehaviour
{
    //private RuleManager _ruleManager;
    [SerializeField]
    private OperationManager _operationManager;

    public void StartTutorial()
    {
        _operationManager.Init(null);
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
}
