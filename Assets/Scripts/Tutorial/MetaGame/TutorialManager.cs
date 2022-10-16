using System.Collections.Generic;
using Account;
using Battle.Data;
using CardMaga.BattleConfigSO;
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

    private TutorialConfigSO _currentTutorialConfig;

    #region PublicFunction
    
    #endregion

    #region PrivateFunction
    
    private void UpdateCurrentBattleConfig()
    {
        _currentTutorialConfig = _tutorialConfig[_currentTutorialIndex];
        
        BattleData.Instance.AssginBattleTutorialData(_currentTutorialConfig);
    }

    private void EndTutorial()
    {
        OnEndTutorial?.Invoke();
    }
    
    private void UpdateTutorialBadges()
    {
        for (int i = 0; i < _currentTutorialIndex; i++)
        {
            _badges[i].TurnOn();
        }
    }

    private void StartTutorial()
    {
        if (_currentTutorialIndex > _tutorialConfig.Count - 1)
        {
            UpdateTutorialBadges();
            EndTutorial();
            return;
        }
        UpdateTutorialBadges();
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

    private void Awake()//temp!
    {
        if (BattleData.Instance != null)
        {
            _currentTutorialIndex = GetBattleTutorialConfigIndex(BattleData.Instance.BattleConfigSO);
        }
        else
        { 
            _currentTutorialIndex = AccountManager.Instance.Data.AccountTutorialData.TutorialProgress;
            _battleDataPrefab = Instantiate(_battleDataPrefab);
        }
        
        for (int i = 0; i < _badges.Length; i++)
        {
            _badges[i].Init();
        }
        
        StartTutorial();
    }

    #endregion
}
