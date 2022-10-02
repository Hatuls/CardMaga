using CardMaga.BattleConfigSO;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using Battle.Data;
using ReiTools.TokenMachine;
using Object = UnityEngine.Object;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialConfigSO> _tutorialConfig;

    private MainMenuTutorial _mianMenuTutorial;

    private TutorialConfigSO _currentTutorialConfig;
    private Object _battleTutorial;

    private TutorialConfigSO TryGetBattleData()
    {
        throw new NotImplementedException();
    }

    private void UpdateCurrentBattleConfig()
    {
        throw new NotImplementedException();
    }

    private void StartTutorial()
    {
        throw new NotImplementedException();
    }
    
//     private void CreateTutorial(ITokenReciever tokenReciever, IBattleManager battleManager)
//     {
// #if UNITY_EDITOR
//         if (_hideTutorial)
//             return;
// #endif
//             
//         if (BattleData.BattleConfigSO?.BattleTutorial == null)
//             return;
//
//         _battleTutorial = Instantiate(BattleData.BattleConfigSO.BattleTutorial);
//     }
}
