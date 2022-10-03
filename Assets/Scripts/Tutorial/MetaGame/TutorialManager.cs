using System.Collections.Generic;
using Account;
using Battle.Data;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static int _currentTutorialIndex = 0;
    
    [SerializeField] private List<TutorialConfigSO> _tutorialConfig;
    [SerializeField] private BattleData _battleDataPrefab;
    [SerializeField] private TutorialBadge[] _badges;

    private MainMenuTutorial _mainMenuTutorial;

    private TutorialConfigSO _currentTutorialConfig;

    #region PublicFunction

    public void NextTutorial()
    {
        _currentTutorialIndex++;
        
        UpdateCurrentBattleConfig();
    }

    #endregion

    #region PrivateFunction

    private void GetBattleData()
    {
        _battleDataPrefab = Instantiate(_battleDataPrefab); 
        
        _battleDataPrefab.AssginBattleTutorialData(_currentTutorialConfig);
    }

    private void UpdateCurrentBattleConfig()
    {
        _currentTutorialConfig = _tutorialConfig[_currentTutorialIndex];

        for (int i = 0; i < _badges.Length; i++)
        {
            _badges[i].Init();
        }
        
        CheckTutorialBadges();
        
        GetBattleData();
    }
    
    private void CheckTutorialBadges()
    {
        for (int i = 0; i < _currentTutorialIndex; i++)
        {
            _badges[i].TurnOn();
        }
    }

    private void StartTutorial()
    {
        _currentTutorialConfig = _tutorialConfig[_currentTutorialIndex];

        for (int i = 0; i < _badges.Length; i++)
        {
            _badges[i].Init();
        }
        
        GetBattleData();
    }

    #endregion

    #region UnityCallback

    private void Awake()//temp!
    {
        _currentTutorialIndex = AccountManager.Instance.Data.AccountTutorialData.TutorialProgress;
        StartTutorial();
    }

    #endregion
}
