using System.Collections.Generic;
using Account;
using Battle.Data;
using CardMaga.BattleConfigSO;
using CardMaga.Tutorial;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEndTutorial;
    
    private static int _currentTutorialIndex = 0;
    
    [SerializeField] private List<TutorialConfigSO> _tutorialConfig;
    [SerializeField] private BattleData _battleDataPrefab;
    [SerializeField] private TutorialBadge[] _badges;

    private MainMenuTutorial _mainMenuTutorial;
    private AccountTutorialData _accountTutorialData;
    private TutorialConfigSO _currentTutorialConfig;

    #region PublicFunction
    
    #endregion

    #region PrivateFunction
    
    private void UpdateCurrentBattleConfig()
    {
        _currentTutorialConfig = _tutorialConfig[_currentTutorialIndex];
        
        BattleData.Instance.AssginBattleTutorialData(_currentTutorialConfig);
    }
    
    private void UpdateCurrentBattleConfig(int tutorialIndex)
    {
        _currentTutorialConfig = _tutorialConfig[tutorialIndex];
        
        BattleData.Instance.AssginBattleTutorialData(_currentTutorialConfig);
    }

    private void EndTutorial()
    {
        AccountManager.Instance.Data.AccountTutorialData.AssignedData(_currentTutorialIndex, true);
        AccountManager.Instance.UpdateDataOnServer();
        OnEndTutorial?.Invoke();
    }
    
    private void StartTutorial()
    {
        if (_currentTutorialIndex > _tutorialConfig.Count - 1)
        {
            EndTutorial();
            return;
        }
        
        UpdateCurrentBattleConfig();
    }

    private int GetBattleTutorialConfigIndex(BattleConfigSO battleConfigSo)
    {
        for (int i = 0; i < _tutorialConfig.Count; i++)
        {
            if (_tutorialConfig[i].BattleConfig == battleConfigSo)
            {
                return i + 1;
            }
        }

        return -1;
    }

    #endregion

    #region UnityCallback

    private void Start()//temp!
    {
        if (BattleData.Instance != null)
        {
            _currentTutorialIndex = GetBattleTutorialConfigIndex(BattleData.Instance.BattleConfigSO);
            AccountManager.Instance.Data.AccountTutorialData.AssignedData(_currentTutorialIndex, false);
        }
        else
        {
            _accountTutorialData = AccountManager.Instance.Data.AccountTutorialData;
            _currentTutorialIndex = _accountTutorialData.TutorialProgress;
            _battleDataPrefab = Instantiate(_battleDataPrefab);
        }

        for (int i = 0; i < _badges.Length; i++)
        {
            if (i < _currentTutorialIndex)
            {
                _badges[i].Completed();
                continue;
            }

            if (i > _currentTutorialIndex)
            {
                _badges[i].Init();
                continue;
            }

            if (i == _currentTutorialIndex)
            {
                _badges[i].Open();
            }

        }

        //StartTutorial();
    }

    #endregion
}
