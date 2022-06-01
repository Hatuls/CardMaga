using System;
using System.Collections;
using System.Collections.Generic;
using UI.Meta.PlayScreen;
using UnityEngine;

public class LoreHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] _panelsToActivate;
    int _currentPage = -1;
    PlayPackage _playpackage = new PlayPackage();
    [SerializeField] Animator _anim;
    private void Start()
    {
        _currentPage = -1;
        OpenNextPanel();
    }

    public void OpenNextPanel()
    {
        OpenPanel();
    }
    private void OpenPanel()
    {
        if (_currentPage >= _panelsToActivate.Length)
            return;
        if (_currentPage >= 0)
            _panelsToActivate[_currentPage].SetActive(false);
        
        _currentPage++;

        if (_currentPage < _panelsToActivate.Length)
        {
            _panelsToActivate[_currentPage].SetActive(true);
            if (_currentPage== 1)
            _anim.SetTrigger("Move");
        }
        else
            StartTutorialBattle();
    }

    private void StartTutorialBattle()
    {
       
            var account = Account.AccountManager.Instance;
            _playpackage.CharacterData = account.AccountCharacters.GetCharacterData(CharacterEnum.Chiara);
            _playpackage.Deck = _playpackage.CharacterData.GetDeckAt(0);
            _playpackage.SendPackage();
            account.BattleData.Opponent = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(Battles.CharacterTypeEnum.Tutorial);
          //  SceneHandler.LoadScene(SceneHandler.ScenesEnum.GameBattleScene);
            account.IsDoneTutorial = false;
        
    }
}
