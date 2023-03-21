using CardMaga.Battle;
using CardMaga.Input;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInputChanger : MonoBehaviour
{
    [SerializeField] private InputGroup _endGameInputGroup;
    private void ChangeInputGroup(ITokenReceiver tokenReciever)
    {
        ChangeInputGroup(_endGameInputGroup);
    }

    public void ChangeInputGroup(InputGroup inputGroup)
    {
        LockAndUnlockSystem.Instance.SetNewInputGroup(inputGroup);
    }

    private void OnEnable()
    {
        BattleManager.Instance.EndBattleHandler.OnBattleFinished += ChangeInputGroup;
    }

    private void OnDestroy()
    {
        BattleManager.Instance.EndBattleHandler.OnBattleFinished -= ChangeInputGroup;
    }
}
