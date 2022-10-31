using Battle.Data;
using UnityEngine;
using UnityEngine.Events;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _moveToMainManu;
    [SerializeField] private UnityEvent _moveToTutorial;
    
    [SerializeField] private VictoryAndDefeatHandler _victoryAndDefeat;
    [SerializeField] private RewardScreenUIHandler _rewardScreen;
    
    private bool _isInTutorial;
    private bool _isLeftPlayerWon;
    
    private void Start()
    {
        _isInTutorial = !(BattleData.Instance.BattleConfigSO.BattleTutorial == null);
        _isLeftPlayerWon = BattleData.Instance.PlayerWon;
        
        _victoryAndDefeat.OpenScreen(_isLeftPlayerWon);
    }
    
    public void ShowReward()
    {
        if (_isLeftPlayerWon)
        {
            _victoryAndDefeat.gameObject.SetActive(false);
            _rewardScreen.gameObject.SetActive(true);
        }
        else
            MoveToNextScene();
    }

    public void MoveToNextScene()
    {
        if (_isInTutorial)
            _moveToTutorial?.Invoke();
        else
            _moveToMainManu?.Invoke();
    }
}
